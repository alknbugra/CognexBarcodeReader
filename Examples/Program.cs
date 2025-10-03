using System;
using System.Threading.Tasks;

namespace CognexBarcodeReader.Examples
{
    /// <summary>
    /// Demo uygulamasının ana giriş noktası
    /// Bu program çeşitli kullanım örneklerini gösterir
    /// </summary>
    class Program
    {
        /// <summary>
        /// Ana giriş noktası
        /// </summary>
        /// <param name="args">Komut satırı argümanları</param>
        static async Task Main(string[] args)
        {
            Console.WriteLine("🔍 Cognex Barcode Reader - Demo Uygulaması");
            Console.WriteLine("=" * 50);
            Console.WriteLine("Bu uygulama çeşitli kullanım örneklerini gösterir.");
            Console.WriteLine();

            // Demo menüsünü göster
            await ShowDemoMenu();
        }

        /// <summary>
        /// Demo menüsünü gösterir
        /// </summary>
        private static async Task ShowDemoMenu()
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
                        await RunBasicExample();
                        break;
                    case '2':
                        await RunAdvancedExample();
                        break;
                    case '3':
                        await RunConsoleExample();
                        break;
                    case '4':
                        RunWindowsFormsExample();
                        break;
                    case '5':
                        ShowAllExamples();
                        break;
                    case '6':
                    case 'q':
                    case 'Q':
                        ExitApplication();
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
            Console.WriteLine("🔍 Cognex Barcode Reader - Demo Uygulaması");
            Console.WriteLine("=" * 50);
            Console.WriteLine("📚 Çeşitli kullanım örneklerini keşfedin");
            Console.WriteLine("=" * 50);
        }

        /// <summary>
        /// Menü seçeneklerini gösterir
        /// </summary>
        private static void ShowMenuOptions()
        {
            Console.WriteLine("📋 Demo Seçenekleri:");
            Console.WriteLine();
            Console.WriteLine("1️⃣  Temel Kullanım Örneği");
            Console.WriteLine("2️⃣  Gelişmiş Kullanım Örneği");
            Console.WriteLine("3️⃣  Konsol Uygulaması Örneği");
            Console.WriteLine("4️⃣  Windows Forms Örneği");
            Console.WriteLine("5️⃣  Tüm Örnekleri Göster");
            Console.WriteLine("6️⃣  Çıkış (Q)");
            Console.WriteLine();
            Console.Write("Seçiminizi yapın (1-6): ");
        }

        /// <summary>
        /// Temel kullanım örneğini çalıştırır
        /// </summary>
        private static async Task RunBasicExample()
        {
            Console.WriteLine("\n🚀 Temel Kullanım Örneği Başlatılıyor...");
            Console.WriteLine("=" * 40);
            
            try
            {
                await BasicUsage.SimpleExample();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hata: {ex.Message}");
            }
            
            Console.WriteLine("\nDevam etmek için herhangi bir tuşa basın...");
            Console.ReadKey();
        }

        /// <summary>
        /// Gelişmiş kullanım örneğini çalıştırır
        /// </summary>
        private static async Task RunAdvancedExample()
        {
            Console.WriteLine("\n🚀 Gelişmiş Kullanım Örneği Başlatılıyor...");
            Console.WriteLine("=" * 40);
            
            Console.WriteLine("Hangi gelişmiş örneği çalıştırmak istiyorsunuz?");
            Console.WriteLine("1. Barkod Geçmişi ve Loglama");
            Console.WriteLine("2. Çoklu Barkod Okuma");
            Console.WriteLine("3. Hata Yönetimi ve Retry");
            Console.WriteLine("4. Konfigürasyon Dosyası");
            Console.Write("Seçiminiz (1-4): ");
            
            var choice = Console.ReadKey(true).KeyChar;
            
            try
            {
                switch (choice)
                {
                    case '1':
                        await AdvancedUsage.BarcodeHistoryExample();
                        break;
                    case '2':
                        await AdvancedUsage.MultipleBarcodeExample();
                        break;
                    case '3':
                        await AdvancedUsage.ErrorHandlingExample();
                        break;
                    case '4':
                        await AdvancedUsage.ConfigurationFileExample();
                        break;
                    default:
                        Console.WriteLine("❌ Geçersiz seçim!");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hata: {ex.Message}");
            }
            
            Console.WriteLine("\nDevam etmek için herhangi bir tuşa basın...");
            Console.ReadKey();
        }

        /// <summary>
        /// Konsol uygulaması örneğini çalıştırır
        /// </summary>
        private static async Task RunConsoleExample()
        {
            Console.WriteLine("\n🚀 Konsol Uygulaması Örneği Başlatılıyor...");
            Console.WriteLine("=" * 40);
            
            try
            {
                await ConsoleApplication.Main(new string[0]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Windows Forms örneğini çalıştırır
        /// </summary>
        private static void RunWindowsFormsExample()
        {
            Console.WriteLine("\n🚀 Windows Forms Örneği Başlatılıyor...");
            Console.WriteLine("=" * 40);
            
            try
            {
                WindowsFormsExample.RunExample();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hata: {ex.Message}");
            }
        }

        /// <summary>
        /// Tüm örnekleri gösterir
        /// </summary>
        private static void ShowAllExamples()
        {
            Console.WriteLine("\n📚 Tüm Örnekler:");
            Console.WriteLine("=" * 30);
            
            Console.WriteLine("\n1️⃣  Temel Kullanım Örnekleri:");
            Console.WriteLine("   • SimpleExample() - En basit kullanım");
            Console.WriteLine("   • CustomConfigurationExample() - Özelleştirilmiş ayarlar");
            
            Console.WriteLine("\n2️⃣  Gelişmiş Kullanım Örnekleri:");
            Console.WriteLine("   • BarcodeHistoryExample() - Geçmiş tutma ve loglama");
            Console.WriteLine("   • MultipleBarcodeExample() - Çoklu barkod okuma");
            Console.WriteLine("   • ErrorHandlingExample() - Hata yönetimi");
            Console.WriteLine("   • ConfigurationFileExample() - Dosyadan ayar yükleme");
            
            Console.WriteLine("\n3️⃣  UI Örnekleri:");
            Console.WriteLine("   • ConsoleApplication - Komut satırı uygulaması");
            Console.WriteLine("   • WindowsFormsExample - Grafik arayüz uygulaması");
            
            Console.WriteLine("\n4️⃣  Kullanım Senaryoları:");
            Console.WriteLine("   • Endüstriyel üretim hattı");
            Console.WriteLine("   • Depo yönetimi");
            Console.WriteLine("   • Envanter takibi");
            Console.WriteLine("   • Kalite kontrol");
            
            Console.WriteLine("\n5️⃣  Entegrasyon Örnekleri:");
            Console.WriteLine("   • Veritabanı kaydetme");
            Console.WriteLine("   • Web API entegrasyonu");
            Console.WriteLine("   • Log dosyası yazma");
            Console.WriteLine("   • İstatistik toplama");
            
            Console.WriteLine("\nDevam etmek için herhangi bir tuşa basın...");
            Console.ReadKey();
        }

        /// <summary>
        /// Uygulamadan çıkar
        /// </summary>
        private static void ExitApplication()
        {
            Console.WriteLine("\n👋 Demo uygulamasından çıkılıyor...");
            Console.WriteLine("✅ CognexBarcodeReader projesini keşfetmeye devam edin!");
            Console.WriteLine("🔗 GitHub: https://github.com/alknbugra/CognexBarcodeReader");
            Environment.Exit(0);
        }
    }
}
