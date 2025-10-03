using Sample2CognexBarcodeReader.Interfaces;
using Sample2CognexBarcodeReader.Models;
using Sample2CognexBarcodeReader.Services;
using System;
using System.Threading.Tasks;

namespace CognexBarcodeReader.Examples
{
    /// <summary>
    /// Konsol uygulaması örneği - Komut satırı tabanlı barkod okuyucu
    /// </summary>
    public class ConsoleApplication
    {
        private static IBarcodeReaderService _barcodeService;
        private static BarcodeReaderConfig _config;
        private static bool _isRunning = false;
        private static int _barcodeCount = 0;

        /// <summary>
        /// Ana konsol uygulaması
        /// </summary>
        public static async Task Main(string[] args)
        {
            Console.WriteLine("🔍 Cognex Barcode Reader - Konsol Uygulaması");
            Console.WriteLine("=" * 50);
            
            // Komut satırı argümanlarını işle
            ProcessCommandLineArguments(args);
            
            // Konfigürasyonu başlat
            InitializeConfiguration();
            
            // Ana menüyü göster
            await ShowMainMenu();
        }

        /// <summary>
        /// Komut satırı argümanlarını işler
        /// </summary>
        private static void ProcessCommandLineArguments(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                switch (args[i].ToLower())
                {
                    case "--help":
                    case "-h":
                        ShowHelp();
                        Environment.Exit(0);
                        break;
                    case "--timeout":
                        if (i + 1 < args.Length && int.TryParse(args[i + 1], out int timeout))
                        {
                            Console.WriteLine($"⚙️  Timeout ayarlandı: {timeout}ms");
                            i++; // Bir sonraki argümanı atla
                        }
                        break;
                    case "--debug":
                        Console.WriteLine("🐛 Debug modu aktif");
                        break;
                    case "--version":
                    case "-v":
                        ShowVersion();
                        Environment.Exit(0);
                        break;
                }
            }
        }

        /// <summary>
        /// Konfigürasyonu başlatır
        /// </summary>
        private static void InitializeConfiguration()
        {
            _config = new BarcodeReaderConfig
            {
                Timeout = 5000,
                Baudrate = 115200,
                DebugMode = true,
                AutoDiscovery = true,
                ThreadSleep = 20
            };
        }

        /// <summary>
        /// Ana menüyü gösterir
        /// </summary>
        private static async Task ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                ShowHeader();
                ShowMenuOptions();
                
                var choice = Console.ReadKey(true).KeyChar;
                
                switch (choice)
                {
                    case '1':
                        await StartBarcodeReading();
                        break;
                    case '2':
                        ShowConfiguration();
                        break;
                    case '3':
                        await TestConnection();
                        break;
                    case '4':
                        ShowStatistics();
                        break;
                    case '5':
                        ShowHelp();
                        break;
                    case '6':
                    case 'q':
                    case 'Q':
                        await ExitApplication();
                        return;
                    default:
                        Console.WriteLine("❌ Geçersiz seçim! Tekrar deneyin...");
                        await Task.Delay(1000);
                        break;
                }
            }
        }

        /// <summary>
        /// Başlık gösterir
        /// </summary>
        private static void ShowHeader()
        {
            Console.WriteLine("🔍 Cognex Barcode Reader - Konsol Uygulaması");
            Console.WriteLine("=" * 50);
            Console.WriteLine($"📊 Okunan Barkod Sayısı: {_barcodeCount}");
            Console.WriteLine($"⚙️  Durum: {(_isRunning ? "Çalışıyor" : "Durduruldu")}");
            Console.WriteLine("=" * 50);
        }

        /// <summary>
        /// Menü seçeneklerini gösterir
        /// </summary>
        private static void ShowMenuOptions()
        {
            Console.WriteLine("📋 Menü Seçenekleri:");
            Console.WriteLine();
            Console.WriteLine("1️⃣  Barkod Okumayı Başlat");
            Console.WriteLine("2️⃣  Konfigürasyonu Göster");
            Console.WriteLine("3️⃣  Bağlantıyı Test Et");
            Console.WriteLine("4️⃣  İstatistikleri Göster");
            Console.WriteLine("5️⃣  Yardım");
            Console.WriteLine("6️⃣  Çıkış (Q)");
            Console.WriteLine();
            Console.Write("Seçiminizi yapın (1-6): ");
        }

        /// <summary>
        /// Barkod okumayı başlatır
        /// </summary>
        private static async Task StartBarcodeReading()
        {
            if (_isRunning)
            {
                Console.WriteLine("⚠️  Servis zaten çalışıyor!");
                Console.WriteLine("Devam etmek için herhangi bir tuşa basın...");
                Console.ReadKey();
                return;
            }

            try
            {
                Console.WriteLine("🚀 Barkod okuma servisi başlatılıyor...");
                
                _barcodeService = new CognexBarcodeReaderService(_config);
                
                // Event'leri bağla
                _barcodeService.DeviceConnected += OnDeviceConnected;
                _barcodeService.DeviceDisconnected += OnDeviceDisconnected;
                _barcodeService.BarcodeRead += OnBarcodeRead;

                var success = await _barcodeService.StartAsync();
                
                if (success)
                {
                    _isRunning = true;
                    Console.WriteLine("✅ Servis başarıyla başlatıldı!");
                    Console.WriteLine("📱 Barkod okumaya başlayın...");
                    Console.WriteLine("⏹️  Durdurmak için 'q' tuşuna basın...");
                    
                    // Kullanıcı 'q' tuşuna basana kadar bekle
                    while (_isRunning)
                    {
                        var key = Console.ReadKey(true);
                        if (key.KeyChar == 'q' || key.KeyChar == 'Q')
                        {
                            await StopBarcodeReading();
                        }
                    }
                }
                else
                {
                    Console.WriteLine("❌ Servis başlatılamadı!");
                    Console.WriteLine("Lütfen cihaz bağlantısını kontrol edin.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hata oluştu: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Devam etmek için herhangi bir tuşa basın...");
                Console.ReadKey();
            }
        }

        /// <summary>
        /// Barkod okumayı durdurur
        /// </summary>
        private static async Task StopBarcodeReading()
        {
            if (!_isRunning) return;

            try
            {
                Console.WriteLine("\n🛑 Servis durduruluyor...");
                
                if (_barcodeService != null)
                {
                    await _barcodeService.StopAsync();
                    _barcodeService.Dispose();
                    _barcodeService = null;
                }
                
                _isRunning = false;
                Console.WriteLine("✅ Servis durduruldu!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Durdurma hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Konfigürasyonu gösterir
        /// </summary>
        private static void ShowConfiguration()
        {
            Console.WriteLine("\n⚙️  Mevcut Konfigürasyon:");
            Console.WriteLine("=" * 30);
            Console.WriteLine($"Timeout: {_config.Timeout}ms");
            Console.WriteLine($"Baudrate: {_config.Baudrate}");
            Console.WriteLine($"Debug Mode: {_config.DebugMode}");
            Console.WriteLine($"Auto Discovery: {_config.AutoDiscovery}");
            Console.WriteLine($"Thread Sleep: {_config.ThreadSleep}ms");
            Console.WriteLine("=" * 30);
            Console.WriteLine("Devam etmek için herhangi bir tuşa basın...");
            Console.ReadKey();
        }

        /// <summary>
        /// Bağlantıyı test eder
        /// </summary>
        private static async Task TestConnection()
        {
            Console.WriteLine("\n🔍 Bağlantı testi yapılıyor...");
            
            try
            {
                var testService = new CognexBarcodeReaderService(_config);
                
                testService.DeviceConnected += (s, e) => 
                    Console.WriteLine("✅ Cihaz bağlandı!");
                
                testService.DeviceDisconnected += (s, e) => 
                    Console.WriteLine("❌ Cihaz bağlantısı kesildi!");

                var success = await testService.StartAsync();
                
                if (success)
                {
                    Console.WriteLine("✅ Bağlantı testi başarılı!");
                    await Task.Delay(3000); // 3 saniye bekle
                }
                else
                {
                    Console.WriteLine("❌ Bağlantı testi başarısız!");
                }
                
                await testService.StopAsync();
                testService.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Test hatası: {ex.Message}");
            }
            
            Console.WriteLine("Devam etmek için herhangi bir tuşa basın...");
            Console.ReadKey();
        }

        /// <summary>
        /// İstatistikleri gösterir
        /// </summary>
        private static void ShowStatistics()
        {
            Console.WriteLine("\n📊 İstatistikler:");
            Console.WriteLine("=" * 20);
            Console.WriteLine($"Toplam Okunan Barkod: {_barcodeCount}");
            Console.WriteLine($"Servis Durumu: {(_isRunning ? "Çalışıyor" : "Durduruldu")}");
            Console.WriteLine($"Uygulama Çalışma Süresi: {DateTime.Now - Process.GetCurrentProcess().StartTime:hh\\:mm\\:ss}");
            Console.WriteLine("=" * 20);
            Console.WriteLine("Devam etmek için herhangi bir tuşa basın...");
            Console.ReadKey();
        }

        /// <summary>
        /// Yardım gösterir
        /// </summary>
        private static void ShowHelp()
        {
            Console.WriteLine("\n📖 Yardım:");
            Console.WriteLine("=" * 20);
            Console.WriteLine("Bu uygulama Cognex DM280X barkod okuyucu ile çalışır.");
            Console.WriteLine();
            Console.WriteLine("Kullanım:");
            Console.WriteLine("1. Cihazı Type-C kablosu ile bilgisayara bağlayın");
            Console.WriteLine("2. 'Barkod Okumayı Başlat' seçeneğini seçin");
            Console.WriteLine("3. Barkod okutun");
            Console.WriteLine("4. 'q' tuşuna basarak durdurun");
            Console.WriteLine();
            Console.WriteLine("Komut Satırı Argümanları:");
            Console.WriteLine("--help, -h     : Bu yardımı gösterir");
            Console.WriteLine("--timeout <ms> : Timeout değerini ayarlar");
            Console.WriteLine("--debug        : Debug modunu aktifleştirir");
            Console.WriteLine("--version, -v  : Versiyon bilgisini gösterir");
            Console.WriteLine("=" * 20);
            Console.WriteLine("Devam etmek için herhangi bir tuşa basın...");
            Console.ReadKey();
        }

        /// <summary>
        /// Versiyon bilgisini gösterir
        /// </summary>
        private static void ShowVersion()
        {
            Console.WriteLine("🔍 Cognex Barcode Reader v2.0.0");
            Console.WriteLine("📅 2025-01-10");
            Console.WriteLine("👨‍💻 Buğra Alkın (@alknbugra)");
        }

        /// <summary>
        /// Uygulamadan çıkar
        /// </summary>
        private static async Task ExitApplication()
        {
            Console.WriteLine("\n👋 Uygulamadan çıkılıyor...");
            
            if (_isRunning)
            {
                await StopBarcodeReading();
            }
            
            Console.WriteLine("✅ Güle güle!");
            Environment.Exit(0);
        }

        #region Event Handlers

        /// <summary>
        /// Cihaz bağlandığında
        /// </summary>
        private static void OnDeviceConnected(object sender, EventArgs e)
        {
            Console.WriteLine("\n🔗 Cihaz bağlandı - Barkod okumaya hazır!");
        }

        /// <summary>
        /// Cihaz bağlantısı kesildiğinde
        /// </summary>
        private static void OnDeviceDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("\n🔌 Cihaz bağlantısı kesildi!");
        }

        /// <summary>
        /// Barkod okunduğunda
        /// </summary>
        private static void OnBarcodeRead(object sender, BarcodeReadEventArgs e)
        {
            _barcodeCount++;
            
            Console.WriteLine($"\n📱 Barkod #{_barcodeCount} Okundu:");
            Console.WriteLine($"   📄 Veri: {e.BarcodeData}");
            Console.WriteLine($"   🏷️  Tip: {e.BarcodeType}");
            Console.WriteLine($"   🆔 ID: {e.ResultId}");
            Console.WriteLine($"   ⏰ Zaman: {e.ReadTime:HH:mm:ss}");
            Console.WriteLine($"   🖼️  Görüntü: {(e.BarcodeImage != null ? "Mevcut" : "Yok")}");
            Console.WriteLine("   " + "─" * 40);
        }

        #endregion
    }
}
