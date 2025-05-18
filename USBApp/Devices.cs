using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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

        // Вызов в конструкторе или при загрузке формы
        public Devices()
        {
            InitializeComponent();
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
            LoadDevices();
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

        private async void LoadDevices()
        {
            overlayPanel.Visible = true; // Активируем оверлей
            string psScriptActive = @"
        $usbDevices = Get-PnpDevice | Where-Object { $_.InstanceId -match '^USB' -and $_.Status -eq 'OK' -and $_.InstanceId -notmatch 'MI_\d{2}' }
    $result = foreach ($device in $usbDevices) {
        $disk = Get-WmiObject Win32_DiskDrive | Where-Object { $_.PNPDeviceID -eq $device.InstanceId -and $_.InterfaceType -eq 'USB' }
        if ($disk -and $disk.Index -ne $null) {
            $logicalDisk = Get-Disk -Number $disk.Index | Get-Partition | Get-Volume | Select-Object DriveLetter, VolumeName, FileSystemLabel
            [PSCustomObject]@{
                DriveLetter  = if ($logicalDisk.DriveLetter) { ""$($logicalDisk.DriveLetter):"" } else { 'Не найдено' }
                VolumeName   = if ($logicalDisk.FileSystemLabel) { $logicalDisk.FileSystemLabel } else { 'Не найдено' }
                Description  = if ($logicalDisk.VolumeName) { $logicalDisk.VolumeName } else { 'Не найдено' }
                DeviceName   = if ($disk.Model) { $disk.Model } else { 'Не найдено' }
                InstanceId   = $device.InstanceId
                SerialNumber = if ($disk.SerialNumber) { $disk.SerialNumber } else { 'Не найдено' }
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
            }
        }
    }
    @($result) | ConvertTo-Json
    $result
    ";

            string psScriptDisabled = @"
        $usbDevices = Get-PnpDevice | Where-Object { $_.InstanceId -match '^USB' -and $_.ConfigManagerErrorCode -eq 22 }
    $result = foreach ($device in $usbDevices) {
        $disk = Get-WmiObject Win32_DiskDrive | Where-Object { $_.PNPDeviceID -eq $device.PNPDeviceID }
        [PSCustomObject]@{
            DeviceName   = $device.Name
            InstanceId   = $device.InstanceId
            Status       = $device.Status
            SerialNumber = if ($disk) { $disk.SerialNumber } else { 'Не найдено' }
            VolumeName   = 'Не найдено'
            Description  = 'Не найдено'
            DriveLetter  = 'Не найдено'
        }
    }
    @($result) | ConvertTo-Json -Compress
    $result
    ";

            DeviceInfo[] activeDevices = null;
            DeviceInfo[] blockedDevices = null;

            if (cachedDevices != null)
            {
                NowDevices.Clear();
                NowDevices.AddRange(cachedDevices);
                // Переходите сразу к обновлению UI и БД
            }
            else
            {
                // Выполняйте PowerShell-запросы и заполняйте cachedDevices
                NowDevices.Clear();
                if (activeDevices != null) NowDevices.AddRange(activeDevices);
                if (blockedDevices != null) NowDevices.AddRange(blockedDevices);
                cachedDevices = new List<DeviceInfo>(NowDevices); // Сохраняем в кэш
            }

            try
            {
                // Асинхронно выполняем PowerShell-запросы
                activeDevices = await Task.Run(() =>
                {
                    using (PowerShell ps = PowerShell.Create())
                    {
                        ps.AddScript(psScriptActive);
                        var outputActive = ps.Invoke();
                        if (ps.HadErrors)
                        {
                            this.Invoke((MethodInvoker)(() => MessageBox.Show("Ошибка PowerShell (активные устройства): " + ps.Streams.Error)));
                            return null;
                        }

                        string jsonActive = outputActive.FirstOrDefault()?.BaseObject.ToString();
                        if (!string.IsNullOrEmpty(jsonActive))
                        {
                            if (jsonActive.StartsWith("["))
                                return Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceInfo[]>(jsonActive);
                            else
                                return new[] { Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceInfo>(jsonActive) };
                        }
                        return null;
                    }
                });

                blockedDevices = await Task.Run(() =>
                {
                    using (PowerShell ps = PowerShell.Create())
                    {
                        ps.AddScript(psScriptDisabled);
                        var outputDisabled = ps.Invoke();
                        if (ps.HadErrors)
                        {
                            this.Invoke((MethodInvoker)(() => MessageBox.Show("Ошибка PowerShell (заблокированные устройства): " + ps.Streams.Error)));
                            return null;
                        }

                        string jsonDisabled = outputDisabled.FirstOrDefault()?.BaseObject.ToString();
                        if (!string.IsNullOrEmpty(jsonDisabled))
                        {
                            if (jsonDisabled.StartsWith("["))
                                return Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceInfo[]>(jsonDisabled);
                            else
                                return new[] { Newtonsoft.Json.JsonConvert.DeserializeObject<DeviceInfo>(jsonDisabled) };
                        }
                        return null;
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки устройств: {ex.Message}");
            }

            if ((activeDevices == null || activeDevices.Length == 0) && (blockedDevices == null || blockedDevices.Length == 0))
            {
                MessageBox.Show("Никаких USB-устройств не вставлено!");
                devicesTable.Clear();
                return;
            }

            // Обновляем NowDevices
            NowDevices.Clear();
            if (activeDevices != null) NowDevices.AddRange(activeDevices);
            if (blockedDevices != null) NowDevices.AddRange(blockedDevices);

            // Логика отображения сообщений
            if (PastCopyNowDevices.Count > 0)
            {
                CopyNowDevices = new List<DeviceInfo>(NowDevices);
                var addedDevices = new List<DeviceInfo>(CopyNowDevices);
                var removedDevices = new List<DeviceInfo>(PastCopyNowDevices);

                // Сравнение и удаление совпадающих устройств
                for (int i = addedDevices.Count - 1; i >= 0; i--)
                {
                    var device = addedDevices[i];
                    var match = removedDevices.FirstOrDefault(d => d.InstanceId == device.InstanceId);
                    if (match != null)
                    {
                        addedDevices.RemoveAt(i);
                        removedDevices.Remove(match);
                    }
                }

                // Формируем сообщение
                StringBuilder message = new StringBuilder();
                if (addedDevices.Count > 0)
                {
                    message.AppendLine("добавлены устройства:");
                    foreach (var device in addedDevices)
                    {
                        message.AppendLine($"- {device.DeviceName} ({device.InstanceId})");
                    }
                }
                if (removedDevices.Count > 0)
                {
                    message.AppendLine("устройства удалены:");
                    foreach (var device in removedDevices)
                    {
                        message.AppendLine($"- {device.DeviceName} ({device.InstanceId})");
                    }
                }
                if (message.Length > 0)
                {
                    MessageBox.Show(message.ToString());
                }
            }

            // Копируем NowDevices в PastCopyNowDevices
            PastCopyNowDevices = new List<DeviceInfo>(NowDevices);


            devicesTable.Clear(); // Очищаем таблицу перед добавлением новых данных

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Добавляем активные устройства в таблицу
                if (activeDevices != null && activeDevices.Length > 0)
                {
                    foreach (var device in activeDevices)
                    {
                        string volumeName = device.VolumeName ?? "Не найдено";
                        string description = device.Description ?? "Не найдено";
                        string deviceName = device.DeviceName ?? "Не найдено";
                        string instanceId = device.InstanceId ?? "Не найдено";
                        string serialNumber = device.SerialNumber ?? "Не найдено";

                        string checkQuery = "SELECT COUNT(*) FROM Devices WHERE InstanceId = @InstanceId";
                        using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@InstanceId", instanceId);
                            int count = (int)cmd.ExecuteScalar();
                            if (count > 0)
                            {
                                string existingQuery = "SELECT VolumeName, Description, DeviceName, InstanceId, SerialNumber FROM Devices WHERE InstanceId = @InstanceId";
                                using (SqlCommand existingCmd = new SqlCommand(existingQuery, conn))
                                {
                                    existingCmd.Parameters.AddWithValue("@InstanceId", instanceId);
                                    using (SqlDataReader reader = existingCmd.ExecuteReader())
                                    {
                                        if (reader.Read())
                                        {
                                            string dbVolumeName = reader["VolumeName"].ToString();
                                            string dbDescription = reader["Description"].ToString();
                                            string dbDeviceName = reader["DeviceName"].ToString();
                                            string dbInstanceId = reader["InstanceId"].ToString();
                                            string dbSerialNumber = reader["SerialNumber"].ToString();
                                            reader.Close(); // Закрываем reader перед выполнением следующего запроса
                                            if (dbDeviceName == deviceName && dbInstanceId == instanceId && dbSerialNumber == serialNumber && (dbVolumeName != volumeName || dbDescription != description))
                                            {
                                                string updateQuery = "UPDATE Devices SET VolumeName = @VolumeName, Description = @Description WHERE InstanceId = @InstanceId";
                                                using (SqlCommand updateCmd = new SqlCommand(updateQuery, conn))
                                                {
                                                    updateCmd.Parameters.AddWithValue("@VolumeName", volumeName);
                                                    updateCmd.Parameters.AddWithValue("@Description", description);
                                                    updateCmd.Parameters.AddWithValue("@InstanceId", instanceId);
                                                    updateCmd.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string insertQuery = "INSERT INTO Devices (VolumeName, Description, DeviceName, InstanceId, SerialNumber, Access) VALUES (@VolumeName, @Description, @DeviceName, @InstanceId, @SerialNumber, 'разрешён')";
                                using (SqlCommand cmdInsert = new SqlCommand(insertQuery, conn))
                                {
                                    cmdInsert.Parameters.AddWithValue("@VolumeName", volumeName);
                                    cmdInsert.Parameters.AddWithValue("@Description", description);
                                    cmdInsert.Parameters.AddWithValue("@DeviceName", deviceName);
                                    cmdInsert.Parameters.AddWithValue("@InstanceId", instanceId);
                                    cmdInsert.Parameters.AddWithValue("@SerialNumber", serialNumber);
                                    cmdInsert.ExecuteNonQuery();
                                }
                            }
                        }

                        // Добавляем в таблицу
                        devicesTable.Rows.Add(volumeName, description, deviceName, instanceId, serialNumber, "разрешён");
                    }
                }

                // Добавляем заблокированные устройства в таблицу
                if (blockedDevices != null && blockedDevices.Length > 0)
                {
                    foreach (var device in blockedDevices)
                    {
                        string instanceId = device.InstanceId ?? "Не найдено";
                        string checkQuery = "SELECT COUNT(*) FROM Devices WHERE InstanceId = @InstanceId";
                        using (SqlCommand cmd = new SqlCommand(checkQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@InstanceId", instanceId);
                            int count = (int)cmd.ExecuteScalar();
                            if (count > 0)
                            {
                                // Добавляем в таблицу
                                string selectQuery = "SELECT VolumeName, Description, DeviceName, InstanceId, SerialNumber, Access FROM Devices WHERE InstanceId = @InstanceId";
                                using (SqlDataAdapter adapter = new SqlDataAdapter(selectQuery, conn))
                                {
                                    adapter.SelectCommand.Parameters.AddWithValue("@InstanceId", instanceId);
                                    DataTable tempTable = new DataTable();
                                    adapter.Fill(tempTable);
                                    foreach (DataRow row in tempTable.Rows)
                                    {
                                        devicesTable.ImportRow(row);
                                    }
                                }
                            }
                        }
                    }
                }

                if (devicesTable.Rows.Count == 0)
                {
                    MessageBox.Show("Никаких USB-устройств не вставлено!");
                }
            }

            var allDevices = new List<DeviceInfo>();
            if (activeDevices != null) allDevices.AddRange(activeDevices);
            if (blockedDevices != null) allDevices.AddRange(blockedDevices);

            if (allDevices.Count == 0)
            {
                MessageBox.Show("Никаких USB-устройств не вставлено!");
                devicesTable.Clear();
                return;
            }
            overlayPanel.Visible = false; // Деактивируем оверлей
        }


        private ManagementEventWatcher _watcher;
        private void StartUSBMonitoring()
        {
            try
            {
                if (_watcher != null) _watcher.Stop(); // Останавливаем, если уже работает
                var query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2 OR EventType = 3");
                _watcher = new ManagementEventWatcher(query);
                _watcher.EventArrived += async (s, e) =>
                {
                    cachedDevices = null; // Сбрасываем кэш при изменении USB
                    await Task.Run(() => this.Invoke((MethodInvoker)LoadDevices));
                };
                _watcher.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при запуске мониторинга USB: {ex.Message}");
            }
        }

        // Остановка мониторинга при закрытии формы
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_watcher != null)
            {
                _watcher.Stop();
                _watcher.Dispose();
                _watcher = null;
            }
            base.OnFormClosing(e);
        }

        private void bSave_Click(object sender, EventArgs e)
        {
            overlayPanel.Visible = true;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                Parallel.ForEach(devicesTable.Rows.Cast<DataRow>(), row =>
                {
                    string instanceId = row["InstanceId"].ToString();
                    string access = row["Access"].ToString();

                    // Обновление БД
                    string updateQuery = "UPDATE Devices SET Access = @Access WHERE InstanceId = @InstanceId";
                    using (SqlConnection innerConn = new SqlConnection(connectionString)) // Отдельное соединение для каждого потока
                    {
                        innerConn.Open();
                        using (SqlCommand cmd = new SqlCommand(updateQuery, innerConn))
                        {
                            cmd.Parameters.AddWithValue("@Access", access);
                            cmd.Parameters.AddWithValue("@InstanceId", instanceId);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    // Выполнение PowerShell-команды
                    using (PowerShell ps = PowerShell.Create())
                    {
                        string psCommand = access == "разрешён"
                            ? $"Enable-PnpDevice -InstanceId '{instanceId}' -Confirm:$false"
                            : $"Disable-PnpDevice -InstanceId '{instanceId}' -Confirm:$false";
                        ps.AddScript(psCommand);
                        ps.Invoke();
                        if (ps.HadErrors)
                        {
                            this.Invoke((MethodInvoker)(() => MessageBox.Show($"Ошибка PowerShell для {instanceId}: {ps.Streams.Error}")));
                        }
                    }
                });
            }

            overlayPanel.Visible = false;
            MessageBox.Show("Изменения сохранены.");
        }

        private static Devices dvcs;
        public static Devices devices
        {
            get
            {
                if (dvcs == null || dvcs.IsDisposed) dvcs = new Devices();
                return dvcs;
            }
        }
        public void ShowForm()
        {
            Show();
            Activate();
        }

        private void Devices_Load(object sender, EventArgs e)
        {

        }

        private void Devices_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = MessageBox.Show("Вы хотите закрыть программу?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes;
        }
    }
}