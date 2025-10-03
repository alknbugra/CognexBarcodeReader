using Sample2CognexBarcodeReader.Interfaces;
using Sample2CognexBarcodeReader.Models;
using Sample2CognexBarcodeReader.Services;
using Sample2CognexBarcodeReader.Utils;
using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample2CognexBarcodeReader
{
    /// <summary>
    /// Ana uygulama formu - Cognex barkod okuyucu arayüzü
    /// </summary>
    public partial class Form1 : Form
    {
        #region Private Fields

        private IBarcodeReaderService _barcodeReaderService;
        private BarcodeReaderConfig _config;

        #endregion

        #region Constructor

        /// <summary>
        /// Form1'in yeni örneğini başlatır
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            InitializeConfiguration();
            InitializeBarcodeReaderService();
            SetupEventHandlers();
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Konfigürasyonu başlatır
        /// </summary>
        private void InitializeConfiguration()
        {
            _config = new BarcodeReaderConfig
            {
                Timeout = 5000,
                Baudrate = 115200,
                AutoDiscovery = true,
                DebugMode = true
            };
        }

        /// <summary>
        /// Barkod okuyucu servisini başlatır
        /// </summary>
        private void InitializeBarcodeReaderService()
        {
            _barcodeReaderService = new CognexBarcodeReaderService(_config);
        }

        /// <summary>
        /// Event handler'ları ayarlar
        /// </summary>
        private void SetupEventHandlers()
        {
            _barcodeReaderService.DeviceConnected += OnDeviceConnected;
            _barcodeReaderService.DeviceDisconnected += OnDeviceDisconnected;
            _barcodeReaderService.BarcodeRead += OnBarcodeRead;
        }

        #endregion

        #region Form Events

        /// <summary>
        /// Form yüklendiğinde çağrılır
        /// </summary>
        private async void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateStatusLabel("Servis başlatılıyor...");
                var success = await _barcodeReaderService.StartAsync();
                
                if (success)
                {
                    UpdateStatusLabel("Servis başlatıldı. Cihaz aranıyor...");
                }
                else
                {
                    UpdateStatusLabel("Servis başlatılamadı!");
                    ShowErrorMessage("Barkod okuyucu servisi başlatılamadı. Lütfen cihazınızı kontrol edin.");
                }
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"Hata: {ex.Message}");
                ShowErrorMessage($"Uygulama başlatılırken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Form kapatılırken çağrılır
        /// </summary>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                _barcodeReaderService?.StopAsync().Wait();
                _barcodeReaderService?.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Form kapatılırken hata oluştu: {ex.Message}");
            }
            
            base.OnFormClosing(e);
        }

        #endregion

        #region Barcode Reader Service Events

        /// <summary>
        /// Cihaz bağlandığında çağrılır
        /// </summary>
        private void OnDeviceConnected(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                UpdateStatusLabel("Cihaz bağlandı. Barkod okumaya hazır!");
                UpdateConnectionStatus(true);
            }));
        }

        /// <summary>
        /// Cihaz bağlantısı kesildiğinde çağrılır
        /// </summary>
        private void OnDeviceDisconnected(object sender, EventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                UpdateStatusLabel("Cihaz bağlantısı kesildi!");
                UpdateConnectionStatus(false);
            }));
        }

        /// <summary>
        /// Barkod okunduğunda çağrılır
        /// </summary>
        private void OnBarcodeRead(object sender, BarcodeReadEventArgs e)
        {
            this.Invoke(new Action(() =>
            {
                DisplayBarcodeResult(e);
            }));
        }

        #endregion

        #region UI Update Methods

        /// <summary>
        /// Barkod sonucunu görüntüler
        /// </summary>
        private void DisplayBarcodeResult(BarcodeReadEventArgs e)
        {
            try
            {
                // Barkod verisini göster
                lbReadString.Text = e.BarcodeData;
                lbReadString.ForeColor = Color.Green;

                // Barkod görüntüsünü göster
                if (e.BarcodeImage != null)
                {
                    DisplayBarcodeImage(e.BarcodeImage);
                }

                // Durum güncelle
                UpdateStatusLabel($"Barkod okundu: {e.BarcodeData} (Tip: {e.BarcodeType})");
            }
            catch (Exception ex)
            {
                UpdateStatusLabel($"Sonuç gösterilirken hata: {ex.Message}");
                ShowErrorMessage($"Barkod sonucu gösterilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Barkod görüntüsünü görüntüler
        /// </summary>
        private void DisplayBarcodeImage(Image barcodeImage)
        {
            try
            {
                // Eski görüntüyü temizle
                if (picResultImage.Image != null)
                {
                    ImageHelper.SafeDisposeImage(picResultImage.Image);
                    picResultImage.Image = null;
                }

                // Yeni görüntüyü boyutlandır ve göster
                var imageSize = ImageHelper.FitImageInControl(barcodeImage.Size, picResultImage.Size);
                var resizedImage = ImageHelper.ResizeImageToBitmap(barcodeImage, imageSize);

                if (resizedImage != null)
                {
                    picResultImage.Image = resizedImage;
                    picResultImage.Invalidate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Görüntü gösterilirken hata oluştu: {ex.Message}");
            }
        }

        /// <summary>
        /// Durum etiketini günceller
        /// </summary>
        private void UpdateStatusLabel(string message)
        {
            // Eğer status label yoksa, read string label'ını kullan
            if (lbReadString != null)
            {
                lbReadString.Text = message;
                lbReadString.ForeColor = Color.Blue;
            }
        }

        /// <summary>
        /// Bağlantı durumunu günceller
        /// </summary>
        private void UpdateConnectionStatus(bool isConnected)
        {
            this.BackColor = isConnected ? Color.LightGreen : Color.LightCoral;
            this.Text = isConnected ? "Cognex Barcode Reader - Bağlı" : "Cognex Barcode Reader - Bağlantı Yok";
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Hata mesajı gösterir
        /// </summary>
        private void ShowErrorMessage(string message)
        {
            MessageBox.Show(message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Bilgi mesajı gösterir
        /// </summary>
        private void ShowInfoMessage(string message)
        {
            MessageBox.Show(message, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Servis durumunu kontrol eder
        /// </summary>
        public bool IsServiceRunning => _barcodeReaderService?.IsRunning ?? false;

        /// <summary>
        /// Cihaz bağlantı durumunu kontrol eder
        /// </summary>
        public bool IsDeviceConnected => _barcodeReaderService?.IsConnected ?? false;

        #endregion
    }
}