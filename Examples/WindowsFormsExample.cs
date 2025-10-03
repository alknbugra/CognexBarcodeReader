using Sample2CognexBarcodeReader.Interfaces;
using Sample2CognexBarcodeReader.Models;
using Sample2CognexBarcodeReader.Services;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CognexBarcodeReader.Examples
{
    /// <summary>
    /// Windows Forms ile kullanım örneği - UI tabanlı barkod okuyucu
    /// </summary>
    public partial class WindowsFormsExample : Form
    {
        #region Private Fields

        private IBarcodeReaderService _barcodeService;
        private BarcodeReaderConfig _config;
        private int _barcodeCount = 0;
        private DateTime _startTime;

        #endregion

        #region UI Controls

        private Button btnStart;
        private Button btnStop;
        private Button btnClear;
        private Label lblStatus;
        private Label lblBarcodeCount;
        private Label lblBarcodeData;
        private PictureBox picBarcodeImage;
        private ListBox lstBarcodeHistory;
        private TextBox txtBarcodeData;
        private GroupBox grpControls;
        private GroupBox grpResults;
        private GroupBox grpHistory;

        #endregion

        #region Constructor

        /// <summary>
        /// Windows Forms örneği formunu başlatır
        /// </summary>
        public WindowsFormsExample()
        {
            InitializeComponent();
            InitializeConfiguration();
            SetupEventHandlers();
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Konfigürasyonu başlatır
        /// </summary>
        private void InitializeConfiguration()
        {
            _config = new BarcodeReaderConfig
            {
                Timeout = 5000,
                Baudrate = 115200,
                DebugMode = true,
                AutoDiscovery = true
            };
        }

        /// <summary>
        /// Event handler'ları ayarlar
        /// </summary>
        private void SetupEventHandlers()
        {
            btnStart.Click += BtnStart_Click;
            btnStop.Click += BtnStop_Click;
            btnClear.Click += BtnClear_Click;
            this.FormClosing += WindowsFormsExample_FormClosing;
        }

        #endregion

        #region UI Initialization

        /// <summary>
        /// UI kontrollerini başlatır
        /// </summary>
        private void InitializeComponent()
        {
            // Form ayarları
            this.Text = "Cognex Barcode Reader - Windows Forms Örneği";
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Ana grup kutuları
            grpControls = new GroupBox
            {
                Text = "Kontroller",
                Location = new Point(10, 10),
                Size = new Size(200, 150)
            };

            grpResults = new GroupBox
            {
                Text = "Sonuçlar",
                Location = new Point(220, 10),
                Size = new Size(350, 300)
            };

            grpHistory = new GroupBox
            {
                Text = "Barkod Geçmişi",
                Location = new Point(10, 170),
                Size = new Size(560, 200)
            };

            // Kontrol butonları
            btnStart = new Button
            {
                Text = "Başlat",
                Location = new Point(10, 30),
                Size = new Size(80, 30),
                BackColor = Color.LightGreen
            };

            btnStop = new Button
            {
                Text = "Durdur",
                Location = new Point(100, 30),
                Size = new Size(80, 30),
                BackColor = Color.LightCoral,
                Enabled = false
            };

            btnClear = new Button
            {
                Text = "Temizle",
                Location = new Point(10, 70),
                Size = new Size(80, 30),
                BackColor = Color.LightBlue
            };

            // Durum etiketleri
            lblStatus = new Label
            {
                Text = "Durum: Hazır",
                Location = new Point(10, 110),
                Size = new Size(180, 20),
                ForeColor = Color.Blue
            };

            lblBarcodeCount = new Label
            {
                Text = "Okunan: 0",
                Location = new Point(10, 130),
                Size = new Size(180, 20)
            };

            // Sonuç alanı
            lblBarcodeData = new Label
            {
                Text = "Barkod Verisi:",
                Location = new Point(10, 30),
                Size = new Size(100, 20)
            };

            txtBarcodeData = new TextBox
            {
                Location = new Point(10, 50),
                Size = new Size(330, 25),
                ReadOnly = true,
                BackColor = Color.LightYellow
            };

            picBarcodeImage = new PictureBox
            {
                Location = new Point(10, 80),
                Size = new Size(330, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White
            };

            // Geçmiş listesi
            lstBarcodeHistory = new ListBox
            {
                Location = new Point(10, 30),
                Size = new Size(540, 160),
                Font = new Font("Consolas", 9)
            };

            // Kontrolleri forma ekle
            grpControls.Controls.AddRange(new Control[] { btnStart, btnStop, btnClear, lblStatus, lblBarcodeCount });
            grpResults.Controls.AddRange(new Control[] { lblBarcodeData, txtBarcodeData, picBarcodeImage });
            grpHistory.Controls.Add(lstBarcodeHistory);

            this.Controls.AddRange(new Control[] { grpControls, grpResults, grpHistory });
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Başlat butonuna tıklandığında
        /// </summary>
        private async void BtnStart_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                lblStatus.Text = "Durum: Başlatılıyor...";
                lblStatus.ForeColor = Color.Orange;

                // Servis oluştur
                _barcodeService = new CognexBarcodeReaderService(_config);
                
                // Event'leri bağla
                _barcodeService.DeviceConnected += OnDeviceConnected;
                _barcodeService.DeviceDisconnected += OnDeviceDisconnected;
                _barcodeService.BarcodeRead += OnBarcodeRead;

                // Servisi başlat
                var success = await _barcodeService.StartAsync();
                
                if (success)
                {
                    lblStatus.Text = "Durum: Çalışıyor - Cihaz aranıyor...";
                    lblStatus.ForeColor = Color.Green;
                    _startTime = DateTime.Now;
                }
                else
                {
                    lblStatus.Text = "Durum: Başlatılamadı!";
                    lblStatus.ForeColor = Color.Red;
                    btnStart.Enabled = true;
                    btnStop.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                lblStatus.Text = "Durum: Hata!";
                lblStatus.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Durdur butonuna tıklandığında
        /// </summary>
        private async void BtnStop_Click(object sender, EventArgs e)
        {
            try
            {
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                lblStatus.Text = "Durum: Durduruluyor...";
                lblStatus.ForeColor = Color.Orange;

                if (_barcodeService != null)
                {
                    await _barcodeService.StopAsync();
                    _barcodeService.Dispose();
                    _barcodeService = null;
                }

                lblStatus.Text = "Durum: Durduruldu";
                lblStatus.ForeColor = Color.Blue;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hata: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Temizle butonuna tıklandığında
        /// </summary>
        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtBarcodeData.Clear();
            picBarcodeImage.Image?.Dispose();
            picBarcodeImage.Image = null;
            lstBarcodeHistory.Items.Clear();
            _barcodeCount = 0;
            lblBarcodeCount.Text = "Okunan: 0";
        }

        /// <summary>
        /// Form kapatılırken
        /// </summary>
        private async void WindowsFormsExample_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_barcodeService != null)
            {
                try
                {
                    await _barcodeService.StopAsync();
                    _barcodeService.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Form kapatılırken hata: {ex.Message}");
                }
            }
        }

        #endregion

        #region Barcode Service Events

        /// <summary>
        /// Cihaz bağlandığında
        /// </summary>
        private void OnDeviceConnected(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                lblStatus.Text = "Durum: Cihaz bağlı - Barkod okumaya hazır!";
                lblStatus.ForeColor = Color.Green;
            }));
        }

        /// <summary>
        /// Cihaz bağlantısı kesildiğinde
        /// </summary>
        private void OnDeviceDisconnected(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                lblStatus.Text = "Durum: Cihaz bağlantısı kesildi!";
                lblStatus.ForeColor = Color.Red;
            }));
        }

        /// <summary>
        /// Barkod okunduğunda
        /// </summary>
        private void OnBarcodeRead(object sender, BarcodeReadEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                try
                {
                    // Barkod sayısını artır
                    _barcodeCount++;
                    lblBarcodeCount.Text = $"Okunan: {_barcodeCount}";

                    // Barkod verisini göster
                    txtBarcodeData.Text = e.BarcodeData;
                    txtBarcodeData.BackColor = Color.LightGreen;

                    // Görüntüyü göster
                    if (e.BarcodeImage != null)
                    {
                        picBarcodeImage.Image?.Dispose();
                        picBarcodeImage.Image = e.BarcodeImage;
                    }

                    // Geçmişe ekle
                    var historyItem = $"[{DateTime.Now:HH:mm:ss}] {e.BarcodeData} ({e.BarcodeType})";
                    lstBarcodeHistory.Items.Insert(0, historyItem);
                    
                    // Maksimum 100 öğe tut
                    if (lstBarcodeHistory.Items.Count > 100)
                    {
                        lstBarcodeHistory.Items.RemoveAt(lstBarcodeHistory.Items.Count - 1);
                    }

                    // Otomatik scroll
                    lstBarcodeHistory.TopIndex = 0;

                    // Başarı mesajı
                    lblStatus.Text = $"Son okuma: {DateTime.Now:HH:mm:ss}";
                    lblStatus.ForeColor = Color.Green;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Barkod işlenirken hata: {ex.Message}", "Hata", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }));
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Windows Forms örneğini çalıştır
        /// </summary>
        public static void RunExample()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WindowsFormsExample());
        }

        #endregion
    }
}
