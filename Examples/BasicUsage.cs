using Sample2CognexBarcodeReader.Interfaces;
using Sample2CognexBarcodeReader.Models;
using Sample2CognexBarcodeReader.Services;
using System;
using System.Threading.Tasks;

namespace CognexBarcodeReader.Examples
{
    /// <summary>
    /// Temel kullanım örneği - CognexBarcodeReader servisinin nasıl kullanılacağını gösterir
    /// </summary>
    public class BasicUsage
    {
        /// <summary>
        /// En basit kullanım örneği - varsayılan ayarlarla servis başlatma
        /// </summary>
        public static async Task SimpleExample()
        {
            Console.WriteLine("=== Temel Kullanım Örneği ===");
            
            // 1. Servis oluştur (varsayılan ayarlarla)
            var barcodeService = new CognexBarcodeReaderService();
            
            // 2. Event'leri bağla
            barcodeService.DeviceConnected += (s, e) => 
                Console.WriteLine("✅ Cihaz bağlandı!");
            
            barcodeService.DeviceDisconnected += (s, e) => 
                Console.WriteLine("❌ Cihaz bağlantısı kesildi!");
            
            barcodeService.BarcodeRead += (s, e) => 
                Console.WriteLine($"📱 Barkod okundu: {e.BarcodeData}");

            try
            {
                // 3. Servisi başlat
                Console.WriteLine("🚀 Servis başlatılıyor...");
                var success = await barcodeService.StartAsync();
                
                if (success)
                {
                    Console.WriteLine("✅ Servis başarıyla başlatıldı!");
                    Console.WriteLine("📱 Barkod okumak için cihazınızı kullanın...");
                    Console.WriteLine("⏹️  Durdurmak için herhangi bir tuşa basın...");
                    
                    // Kullanıcı tuşuna basana kadar bekle
                    Console.ReadKey();
                }
                else
                {
                    Console.WriteLine("❌ Servis başlatılamadı!");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hata oluştu: {ex.Message}");
            }
            finally
            {
                // 4. Servisi durdur ve kaynakları temizle
                Console.WriteLine("🛑 Servis durduruluyor...");
                await barcodeService.StopAsync();
                barcodeService.Dispose();
                Console.WriteLine("✅ Servis durduruldu ve kaynaklar temizlendi.");
            }
        }

        /// <summary>
        /// Özelleştirilmiş konfigürasyon ile kullanım örneği
        /// </summary>
        public static async Task CustomConfigurationExample()
        {
            Console.WriteLine("=== Özelleştirilmiş Konfigürasyon Örneği ===");
            
            // 1. Özel konfigürasyon oluştur
            var config = new BarcodeReaderConfig
            {
                Timeout = 10000,        // 10 saniye timeout
                Baudrate = 115200,      // Baudrate ayarı
                DebugMode = true,       // Debug logları aktif
                AutoDiscovery = true,   // Otomatik cihaz keşfi
                ThreadSleep = 50        // Thread sleep süresi
            };

            // 2. Servis oluştur
            var barcodeService = new CognexBarcodeReaderService(config);
            
            // 3. Event'leri bağla
            barcodeService.DeviceConnected += OnDeviceConnected;
            barcodeService.DeviceDisconnected += OnDeviceDisconnected;
            barcodeService.BarcodeRead += OnBarcodeRead;

            try
            {
                // 4. Servisi başlat
                Console.WriteLine("🚀 Özelleştirilmiş servis başlatılıyor...");
                var success = await barcodeService.StartAsync();
                
                if (success)
                {
                    Console.WriteLine("✅ Servis başarıyla başlatıldı!");
                    Console.WriteLine($"⚙️  Timeout: {config.Timeout}ms");
                    Console.WriteLine($"⚙️  Baudrate: {config.Baudrate}");
                    Console.WriteLine($"⚙️  Debug Mode: {config.DebugMode}");
                    
                    // 5 saniye bekle
                    await Task.Delay(5000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hata oluştu: {ex.Message}");
            }
            finally
            {
                // 5. Temizlik
                await barcodeService.StopAsync();
                barcodeService.Dispose();
            }
        }

        /// <summary>
        /// Cihaz bağlandığında çağrılır
        /// </summary>
        private static void OnDeviceConnected(object sender, EventArgs e)
        {
            Console.WriteLine("🔗 Cihaz bağlandı - Barkod okumaya hazır!");
        }

        /// <summary>
        /// Cihaz bağlantısı kesildiğinde çağrılır
        /// </summary>
        private static void OnDeviceDisconnected(object sender, EventArgs e)
        {
            Console.WriteLine("🔌 Cihaz bağlantısı kesildi!");
        }

        /// <summary>
        /// Barkod okunduğunda çağrılır
        /// </summary>
        private static void OnBarcodeRead(object sender, BarcodeReadEventArgs e)
        {
            Console.WriteLine($"📱 Barkod Okundu:");
            Console.WriteLine($"   📄 Veri: {e.BarcodeData}");
            Console.WriteLine($"   🏷️  Tip: {e.BarcodeType}");
            Console.WriteLine($"   🆔 ID: {e.ResultId}");
            Console.WriteLine($"   ⏰ Zaman: {e.ReadTime:HH:mm:ss}");
            Console.WriteLine($"   🖼️  Görüntü: {(e.BarcodeImage != null ? "Mevcut" : "Yok")}");
            Console.WriteLine("---");
        }
    }
}
