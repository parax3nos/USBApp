using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Management.Automation;

namespace USBApp
{
    public partial class Devices : Form
    {
        private readonly string connectionString = "Server=EVGENY;Database=USBApp;Trusted_Connection=True;"; // Строка подключения
        private DataTable devicesTable;
        private List<DeviceInfo> NowDevices = new List<DeviceInfo>();
        private List<DeviceInfo> CopyNowDevices = new List<DeviceInfo>();
        private List<DeviceInfo> PastCopyNowDevices = new List<DeviceInfo>();
        private Panel overlayPanel;
        private List<DeviceInfo> cachedDevices;
        private System.Windows.Forms.Timer debounceTimer;
        private readonly string logFilePath = "usb_log.txt"; // Путь к лог-файлу
        private bool isAuthorized = false; // Флаг авторизации
        private readonly EnterForm enterForm;

        // Вызов в конструкторе или при загрузке формы
        public Devices(EnterForm enterForm, bool authorized = false)
        {
            this.enterForm = enterForm;
            isAuthorized = authorized; // Устанавливаем флаг авторизации
            InitializeComponent();
            debounceTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000 // Задержка в 1000 мс
            };
            debounceTimer.Tick += (s, e) =>
            {
                debounceTimer.Stop(); // Останавливаем таймер
                LoadDevices();        // Вызываем загрузку устройств
            };
            overlayPanel = new Panel
            {
                BackColor = Color.FromArgb(128, 0, 0, 0), // Полупрозрачный серый
                Dock = DockStyle.Fill,
                Visible = false
            };
            this.Controls.Add(overlayPanel);
            overlayPanel.BringToFront();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.Manual;
            var screen = Screen.PrimaryScreen.WorkingArea;
            this.Location = new Point(
                (screen.Width - this.Width) / 2,
                0);
            InitializeDataGridView();
            SetAuthorizationUI();
            LoadDevices();
            this.Load += Devices_Load;
        }

        private void Devices_Load(object sender, EventArgs e)
        {
            StartUSBMonitoring();
        }

        private void InitializeDataGridView()
        {
            // Настройка DataGridView
            dataGridViewDevices.AutoGenerateColumns = false;

            // Создание и настройка столбца VolumeName
            var volumeNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "VolumeName",
                HeaderText = "Название тома",
                DataPropertyName = "VolumeName",
                ReadOnly = true, // Только чтение
                Width = 150
            };
            volumeNameColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            volumeNameColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Выравнивание заголовка
            dataGridViewDevices.Columns.Add(volumeNameColumn);

            // Создание и настройка столбца Description
            var descriptionColumn = new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Описание",
                DataPropertyName = "Description",
                ReadOnly = true, // Только чтение
                Width = 120
            };
            descriptionColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            descriptionColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Выравнивание заголовка
            dataGridViewDevices.Columns.Add(descriptionColumn);

            // Создание и настройка столбца DeviceName
            var deviceNameColumn = new DataGridViewTextBoxColumn
            {
                Name = "DeviceName",
                HeaderText = "Имя устройства",
                DataPropertyName = "DeviceName",
                ReadOnly = true, // Только чтение
                Width = 200
            };
            deviceNameColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            deviceNameColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Выравнивание заголовка
            dataGridViewDevices.Columns.Add(deviceNameColumn);

            // Создание и настройка столбца InstanceId
            var instanceIdColumn = new DataGridViewTextBoxColumn
            {
                Name = "InstanceId",
                HeaderText = "InstanceId",
                DataPropertyName = "InstanceId",
                ReadOnly = true, // Только чтение
                Width = 510
            };
            instanceIdColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            instanceIdColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Выравнивание заголовка
            dataGridViewDevices.Columns.Add(instanceIdColumn);

            // Создание и настройка столбца SerialNumber
            var serialNumberColumn = new DataGridViewTextBoxColumn
            {
                Name = "SerialNumber",
                HeaderText = "Серийный номер",
                DataPropertyName = "SerialNumber",
                ReadOnly = true, // Только чтение
                Width = 150
            };
            serialNumberColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            serialNumberColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Выравнивание заголовка
            dataGridViewDevices.Columns.Add(serialNumberColumn);

            // Создание и настройка столбца Access
            var accessColumn = new DataGridViewComboBoxColumn
            {
                Name = "Access",
                HeaderText = "Доступ",
                DataPropertyName = "Access",
                DataSource = new[] { "разрешён", "запрещён" },
                ReadOnly = false, // Разрешено редактирование
                Width = 100
            };
            accessColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            accessColumn.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter; // Выравнивание заголовка
            dataGridViewDevices.Columns.Add(accessColumn);

            // Создание DataTable для хранения 
            devicesTable = new DataTable();
            devicesTable.Columns.Add("VolumeName", typeof(string));
            devicesTable.Columns.Add("Description", typeof(string));
            devicesTable.Columns.Add("DeviceName", typeof(string));
            devicesTable.Columns.Add("InstanceId", typeof(string));
            devicesTable.Columns.Add("SerialNumber", typeof(string));
            devicesTable.Columns.Add("Access", typeof(string));

            dataGridViewDevices.DataSource = devicesTable;
            dataGridViewDevices.DataError += (s, e) =>
            {
                e.Cancel = true; // Игнорируем ошибку
            };
        }

        public class DeviceInfo
        {
            public string DriveLetter { get; set; }
            public string VolumeName { get; set; }
            public string Description { get; set; }
            public string DeviceName { get; set; }
            public string InstanceId { get; set; }
            public string SerialNumber { get; set; }
            public string Status { get; set; } // Для заблокированных устройств
        }

        // Метод для логирования событий
        private void LogEvent(string eventType, string instanceId, string deviceName)
        {
            string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {eventType} - InstanceId: {instanceId}, DeviceName: {deviceName}";
            try
            {
                File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка записи в лог: {ex.Message}");
            }
        }

        private void SetAuthorizationUI()
        {
            if (isAuthorized)
            {
                bSave.Visible = true; // Показываем кнопку "Сохранить изменения"
                bAuth.Visible = false; // Скрываем кнопку "Авторизация"
                dataGridViewDevices.Columns["Access"].ReadOnly = false; // Разрешаем редактирование ComboBox
            }
            else
            {
                bSave.Visible = false; // Скрываем кнопку "Сохранить изменения"
                bAuth.Visible = true; // Показываем кнопку "Авторизация"
                dataGridViewDevices.Columns["Access"].ReadOnly = true; // Запрещаем редактирование ComboBox
            }
        }

        private async void LoadDevices()
        {
            overlayPanel.Visible = true;

            // Единый PowerShell-скрипт для активных и заблокированных устройств
            string psScript = @"
    $activeDevices = Get-PnpDevice | Where-Object { $_.InstanceId -match '^USB' -and $_.Status -eq 'OK' -and $_.InstanceId -notmatch 'MI_\d{2}' }
    $disabledDevices = Get-PnpDevice | Where-Object { $_.InstanceId -match '^USB' -and $_.ConfigManagerErrorCode -eq 22 }
    $result = @($activeDevices + $disabledDevices) | ForEach-Object {
        $device = $_
        $disk = Get-WmiObject Win32_DiskDrive | Where-Object { $_.PNPDeviceID -eq $device.InstanceId -and $_.InterfaceType -eq 'USB' }
        if ($device.Status -eq 'OK' -and $disk -and $disk.Index -ne $null) {
            $logicalDisk = Get-Disk -Number $disk.Index | Get-Partition | Get-Volume | Select-Object DriveLetter, VolumeName, FileSystemLabel
            [PSCustomObject]@{
                DriveLetter  = if ($logicalDisk.DriveLetter) { ""$($logicalDisk.DriveLetter):"" } else { 'Не найдено' }
                VolumeName   = if ($logicalDisk.FileSystemLabel) { $logicalDisk.FileSystemLabel } else { 'Не найдено' }
                Description  = if ($logicalDisk.VolumeName) { $logicalDisk.VolumeName } else { 'Не найдено' }
                DeviceName   = if ($disk.Model) { $disk.Model } else { 'Не найдено' }
                InstanceId   = $device.InstanceId
                SerialNumber = if ($disk.SerialNumber) { $disk.SerialNumber } else { 'Не найдено' }
                Status       = $device.Status
            }
        } else {
            $serial = (Get-WmiObject Win32_PnPEntity | Where-Object { $_.PNPDeviceID -eq $device.InstanceId }).SerialNumber
            [PSCustomObject]@{
                DriveLetter  = 'Не найдено'
                VolumeName   = 'Не найдено'
                Description  = 'Не найдено'
                DeviceName   = $device.Name
                InstanceId   = $device.InstanceId
                SerialNumber = if ($serial) { $serial } else { 'Не найдено' }
                Status       = $device.Status
            }
        }
    }
    $result | ConvertTo-Json
";

            DeviceInfo[] devices = null;
            try
            {
                devices = await Task.Run(() =>
                {
                    using (PowerShell ps = PowerShell.Create())
                    {
                        ps.AddScript(psScript);
                        var output = ps.Invoke();
                        if (ps.HadErrors)
                        {
                            this.Invoke((MethodInvoker)(() => MessageBox.Show("Ошибка PowerShell: " + ps.Streams.Error)));
                            return null;
                        }

                        string json = output.FirstOrDefault()?.BaseObject.ToString();
                        if (!string.IsNullOrEmpty(json))
                        {
                            return json.StartsWith("[")
                                ? Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceInfo[]>(json)
                                : new[] { Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceInfo>(json) };
                        }
                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки устройств: {ex.Message}");
                overlayPanel.Visible = false;
                return;
            }

            if (devices == null || devices.Length == 0)
            {
                devicesTable.Clear();
                MessageBox.Show("Никаких USB-устройств не вставлено!");
                overlayPanel.Visible = false;
                return;
            }

            // Обновляем NowDevices и PastCopyNowDevices
            NowDevices.Clear();
            NowDevices.AddRange(devices);

            // Проверяем изменения для уведомлений и логирования
            if (PastCopyNowDevices.Count > 0)
            {
                var addedDevices = NowDevices.Where(nd => !PastCopyNowDevices.Any(pd => pd.InstanceId == nd.InstanceId)).ToList();
                var removedDevices = PastCopyNowDevices.Where(pd => !NowDevices.Any(nd => nd.InstanceId == pd.InstanceId)).ToList();

                StringBuilder message = new StringBuilder();
                if (addedDevices.Count > 0)
                {
                    message.AppendLine("Устройства добавлены:");
                    foreach (var device in addedDevices)
                    {
                        message.AppendLine($"- {device.DeviceName} ({device.InstanceId})");
                        LogEvent("Подключено", device.InstanceId, device.DeviceName); // Логирование подключения
                    }
                }
                if (removedDevices.Count > 0)
                {
                    message.AppendLine("Устройства удалены:");
                    foreach (var device in removedDevices)
                    {
                        message.AppendLine($"- {device.DeviceName} ({device.InstanceId})");
                        LogEvent("Отключено", device.InstanceId, device.DeviceName); // Логирование отключения
                    }
                }
                if (message.Length > 0)
                {
                    MessageBox.Show(message.ToString());
                }
            }

            PastCopyNowDevices.Clear();
            PastCopyNowDevices.AddRange(NowDevices);

            // Подготовка данных с учётом БД
            devicesTable.Clear();
            var devicesToUpdate = new List<(string VolumeName, string Description, string DeviceName, string InstanceId, string SerialNumber, string Access)>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (var device in devices)
                {
                    string instanceId = device.InstanceId ?? "Не найдено";
                    string volumeName = device.VolumeName ?? "Не найдено";
                    string description = device.Description ?? "Не найдено";
                    string deviceName = device.DeviceName ?? "Не найдено";
                    string serialNumber = device.SerialNumber ?? "Не найдено";
                    string access = "разрешён"; // Значение по умолчанию

                    // Проверяем данные в БД
                    string selectQuery = "SELECT VolumeName, Description, DeviceName, SerialNumber, Access FROM Devices WHERE InstanceId = @InstanceId";
                    using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@InstanceId", instanceId);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                volumeName = reader["VolumeName"].ToString() ?? volumeName;
                                description = reader["Description"].ToString() ?? description;
                                deviceName = reader["DeviceName"].ToString() ?? deviceName;
                                serialNumber = reader["SerialNumber"].ToString() ?? serialNumber;
                                access = reader["Access"].ToString() ?? access;
                            }
                        }
                    }

                    devicesTable.Rows.Add(volumeName, description, deviceName, instanceId, serialNumber, access);
                    devicesToUpdate.Add((volumeName, description, deviceName, instanceId, serialNumber, access));
                }

                // Пакетное обновление базы данных
                foreach (var device in devicesToUpdate)
                {
                    string checkQuery = "SELECT COUNT(*) FROM Devices WHERE InstanceId = @InstanceId";
                    using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@InstanceId", device.InstanceId);
                        int count = (int)cmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // Обновляем существующие устройства, но не трогаем Access
                            string updateQuery = "UPDATE Devices SET VolumeName = @VolumeName, Description = @Description, DeviceName = @DeviceName, SerialNumber = @SerialNumber, Access = @Access WHERE InstanceId = @InstanceId";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@VolumeName", device.VolumeName);
                                updateCmd.Parameters.AddWithValue("@Description", device.Description);
                                updateCmd.Parameters.AddWithValue("@DeviceName", device.DeviceName);
                                updateCmd.Parameters.AddWithValue("@SerialNumber", device.SerialNumber);
                                updateCmd.Parameters.AddWithValue("@Access", device.Access);
                                updateCmd.Parameters.AddWithValue("@InstanceId", device.InstanceId);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // Устанавливаем доступ в зависимости от состояния чекбокса в EnterForm
                            string access = enterForm.BlockNewDevices ? "запрещён" : "разрешён";
                            // Выполняем PowerShell-команду для нового устройства
                            if (enterForm.BlockNewDevices)
                            {
                                using (PowerShell ps = PowerShell.Create())
                                {
                                    string psCommand = $"Disable-PnpDevice -InstanceId '{device.InstanceId}' -Confirm:$false";
                                    ps.AddScript(psCommand);
                                    ps.Invoke();
                                    if (ps.HadErrors)
                                    {
                                        this.Invoke((MethodInvoker)(() => MessageBox.Show($"Ошибка PowerShell для {device.InstanceId}: {ps.Streams.Error}")));
                                        LogEvent("Ошибка блокировки", device.InstanceId, device.DeviceName);
                                    }
                                    else
                                    {
                                        LogEvent("Заблокировано", device.InstanceId, device.DeviceName);
                                    }
                                }
                            }
                            // Вставляем новое устройство с установленным доступом
                            string insertQuery = "INSERT INTO Devices (VolumeName, Description, DeviceName, InstanceId, SerialNumber, Access) VALUES (@VolumeName, @Description, @DeviceName, @InstanceId, @SerialNumber, @Access)";
                            using (SqlCommand insertCmd = new SqlCommand(insertQuery, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@VolumeName", device.VolumeName);
                                insertCmd.Parameters.AddWithValue("@Description", device.Description);
                                insertCmd.Parameters.AddWithValue("@DeviceName", device.DeviceName);
                                insertCmd.Parameters.AddWithValue("@InstanceId", device.InstanceId);
                                insertCmd.Parameters.AddWithValue("@SerialNumber", device.SerialNumber);
                                insertCmd.Parameters.AddWithValue("@Access", access);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }

            overlayPanel.Visible = false;
        }


        private ManagementEventWatcher _watcher;
        private void StartUSBMonitoring()
        {
            try
            {
                if (_watcher != null) _watcher.Stop(); // Останавливаем, если уже работает
                var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 OR EventType = 3");
                _watcher = new ManagementEventWatcher(query);
                _watcher.EventArrived += (s, e) =>
                {
                    if (!debounceTimer.Enabled) // Если таймер ещё не запущен
                    {
                        cachedDevices = null;   // Сбрасываем кэш, если он есть
                        this.Invoke((MethodInvoker)debounceTimer.Start); // Вызов в потоке UI
                    }
                };
                _watcher.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске мониторинга USB: {ex.Message}");
            }
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            bool changesMade = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                foreach (DataRow row in devicesTable.Rows)
                {
                    string instanceId = row["InstanceId"].ToString();
                    string newAccess = row["Access"].ToString();

                    // Проверяем текущее значение в БД
                    string selectQuery = "SELECT Access FROM Devices WHERE InstanceId = @InstanceId";
                    using (SqlCommand selectCmd = new SqlCommand(selectQuery, conn))
                    {
                        selectCmd.Parameters.AddWithValue("@InstanceId", instanceId);
                        object result = selectCmd.ExecuteScalar();
                        string currentAccess = result != null ? result.ToString() : null;

                        // Если настройка изменилась
                        if (currentAccess != newAccess)
                        {
                            if (!changesMade)
                            {
                                overlayPanel.Visible = true;
                                changesMade = true;
                            }

                            // Логирование блокировки или разблокировки
                            string eventType = newAccess == "разрешён" ? "Разблокировано" : "Заблокировано";
                            string deviceName = row["DeviceName"].ToString();
                            LogEvent(eventType, instanceId, deviceName);

                            // Обновление БД
                            string updateQuery = "UPDATE Devices SET Access = @Access WHERE InstanceId = @InstanceId";
                            using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@Access", newAccess);
                                updateCmd.Parameters.AddWithValue("@InstanceId", instanceId);
                                updateCmd.ExecuteNonQuery();
                            }

                            // Выполнение PowerShell-команды
                            using (PowerShell ps = PowerShell.Create())
                            {
                                string psCommand = newAccess == "разрешён"
                                    ? $"Enable-PnpDevice -InstanceId '{instanceId}' -Confirm:$false"
                                    : $"Disable-PnpDevice -InstanceId '{instanceId}' -Confirm:$false";
                                ps.AddScript(psCommand);
                                ps.Invoke();
                                if (ps.HadErrors)
                                {
                                    this.Invoke((MethodInvoker)(() => MessageBox.Show($"Ошибка PowerShell для {instanceId}: {ps.Streams.Error}")));
                                }
                            }
                        }
                    }
                }
            }

            if (changesMade)
            {
                overlayPanel.Visible = false;
                MessageBox.Show("Изменения сохранены.");
            }
            else
            {
                MessageBox.Show("Настройки доступа не изменились.");
            }
        }

        private void Devices_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = MessageBox.Show("Вы хотите закрыть программу?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes;
        }

        private void bAuth_Click(object sender, EventArgs e)
        {
            using (var authForm = new LoginForm())
            {
                if (authForm.ShowDialog() == DialogResult.OK)
                {
                    isAuthorized = true;
                    SetAuthorizationUI();
                }
            }
        }

        private void Devices_Load_1(object sender, EventArgs e)
        {

        }
    }
}