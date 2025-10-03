using Sample2CognexBarcodeReader.Interfaces;
using Sample2CognexBarcodeReader.Models;
using Sample2CognexBarcodeReader.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CognexBarcodeReader.Examples
{
    /// <summary>
    /// Gelişmiş kullanım örnekleri - Daha karmaşık senaryolar için
    /// </summary>
    public class AdvancedUsage
    {
        private static List<BarcodeReadEventArgs> _barcodeHistory = new List<BarcodeReadEventArgs>();
        private static string _logFilePath = "barcode_log.txt";

        /// <summary>
        /// Barkod geçmişi tutma ve dosyaya kaydetme örneği
        /// </summary>
        public static async Task BarcodeHistoryExample()
        {
            Console.WriteLine("=== Barkod Geçmişi ve Loglama Örneği ===");
            
            var config = new BarcodeReaderConfig
            {
                DebugMode = true,
                Timeout = 5000
            };

            var barcodeService = new CognexBarcodeReaderService(config);
            
            // Event'leri bağla
            barcodeService.BarcodeRead += OnBarcodeReadWithHistory;
            barcodeService.DeviceConnected += (s, e) => Console.WriteLine("✅ Cihaz bağlandı!");
            barcodeService.DeviceDisconnected += (s, e) => Console.WriteLine("❌ Cihaz bağlantısı kesildi!");

            try
            {
                await barcodeService.StartAsync();
                Console.WriteLine("📱 Barkod okumaya başlayın... (10 saniye)");
                
                // 10 saniye bekle
                await Task.Delay(10000);
                
                // Geçmişi göster
                ShowBarcodeHistory();
                
                // Dosyaya kaydet
                await SaveBarcodeHistoryToFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hata: {ex.Message}");
            }
            finally
            {
                await barcodeService.StopAsync();
                barcodeService.Dispose();
            }
        }

        /// <summary>
        /// Çoklu barkod okuma ve istatistik örneği
        /// </summary>
        public static async Task MultipleBarcodeExample()
        {
            Console.WriteLine("=== Çoklu Barkod Okuma Örneği ===");
            
            var barcodeService = new CognexBarcodeReaderService();
            var barcodeCounts = new Dictionary<string, int>();
            var startTime = DateTime.Now;

            barcodeService.BarcodeRead += (s, e) =>
            {
                // Barkod tipine göre say
                if (barcodeCounts.ContainsKey(e.BarcodeType))
                    barcodeCounts[e.BarcodeType]++;
                else
                    barcodeCounts[e.BarcodeType] = 1;

                Console.WriteLine($"📱 #{barcodeCounts.Values.Sum()}: {e.BarcodeData} ({e.BarcodeType})");
            };

            try
            {
                await barcodeService.StartAsync();
                Console.WriteLine("📱 Birden fazla barkod okutun... (15 saniye)");
                
                await Task.Delay(15000);
                
                // İstatistikleri göster
                ShowBarcodeStatistics(barcodeCounts, startTime);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hata: {ex.Message}");
            }
            finally
            {
                await barcodeService.StopAsync();
                barcodeService.Dispose();
            }
        }

        /// <summary>
        /// Hata yönetimi ve retry mekanizması örneği
        /// </summary>
        public static async Task ErrorHandlingExample()
        {
            Console.WriteLine("=== Hata Yönetimi ve Retry Örneği ===");
            
            var maxRetries = 3;
            var retryCount = 0;
            var success = false;

            while (retryCount < maxRetries && !success)
            {
                try
                {
                    Console.WriteLine($"🔄 Deneme {retryCount + 1}/{maxRetries}");
                    
                    var barcodeService = new CognexBarcodeReaderService();
                    barcodeService.BarcodeRead += (s, e) => 
                        Console.WriteLine($"✅ Barkod okundu: {e.BarcodeData}");

                    success = await barcodeService.StartAsync();
                    
                    if (success)
                    {
                        Console.WriteLine("✅ Servis başarıyla başlatıldı!");
                        await Task.Delay(5000); // 5 saniye çalıştır
                        await barcodeService.StopAsync();
                    }
                    else
                    {
                        Console.WriteLine("❌ Servis başlatılamadı!");
                    }
                    
                    barcodeService.Dispose();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"💥 Hata (Deneme {retryCount + 1}): {ex.Message}");
                    retryCount++;
                    
                    if (retryCount < maxRetries)
                    {
                        Console.WriteLine("⏳ 2 saniye bekleyip tekrar denenecek...");
                        await Task.Delay(2000);
                    }
                }
            }

            if (!success)
            {
                Console.WriteLine("❌ Tüm denemeler başarısız oldu!");
            }
        }

        /// <summary>
        /// Konfigürasyon dosyasından ayarları yükleme örneği
        /// </summary>
        public static async Task ConfigurationFileExample()
        {
            Console.WriteLine("=== Konfigürasyon Dosyası Örneği ===");
            
            // Konfigürasyon dosyası oluştur
            await CreateConfigurationFile();
            
            // Konfigürasyonu yükle
            var config = LoadConfigurationFromFile();
            
            Console.WriteLine($"⚙️  Yüklenen ayarlar:");
            Console.WriteLine($"   Timeout: {config.Timeout}ms");
            Console.WriteLine($"   Baudrate: {config.Baudrate}");
            Console.WriteLine($"   Debug Mode: {config.DebugMode}");
            Console.WriteLine($"   Auto Discovery: {config.AutoDiscovery}");

            var barcodeService = new CognexBarcodeReaderService(config);
            
            try
            {
                await barcodeService.StartAsync();
                Console.WriteLine("✅ Konfigürasyon dosyasından ayarlar yüklendi!");
                await Task.Delay(3000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Hata: {ex.Message}");
            }
            finally
            {
                await barcodeService.StopAsync();
                barcodeService.Dispose();
            }
        }

        #region Helper Methods

        /// <summary>
        /// Barkod okunduğunda geçmişe ekle
        /// </summary>
        private static void OnBarcodeReadWithHistory(object sender, BarcodeReadEventArgs e)
        {
            _barcodeHistory.Add(e);
            Console.WriteLine($"📱 Barkod #{_barcodeHistory.Count}: {e.BarcodeData}");
        }

        /// <summary>
        /// Barkod geçmişini göster
        /// </summary>
        private static void ShowBarcodeHistory()
        {
            Console.WriteLine($"\n📊 Barkod Geçmişi ({_barcodeHistory.Count} adet):");
            Console.WriteLine("=" * 50);
            
            for (int i = 0; i < _barcodeHistory.Count; i++)
            {
                var barcode = _barcodeHistory[i];
                Console.WriteLine($"{i + 1:D2}. {barcode.BarcodeData} ({barcode.BarcodeType}) - {barcode.ReadTime:HH:mm:ss}");
            }
        }

        /// <summary>
        /// Barkod geçmişini dosyaya kaydet
        /// </summary>
        private static async Task SaveBarcodeHistoryToFile()
        {
            try
            {
                using (var writer = new StreamWriter(_logFilePath, true))
                {
                    await writer.WriteLineAsync($"\n=== Barkod Log - {DateTime.Now:yyyy-MM-dd HH:mm:ss} ===");
                    
                    foreach (var barcode in _barcodeHistory)
                    {
                        await writer.WriteLineAsync($"{barcode.ReadTime:HH:mm:ss} - {barcode.BarcodeData} ({barcode.BarcodeType})");
                    }
                    
                    await writer.WriteLineAsync("=" * 50);
                }
                
                Console.WriteLine($"💾 Geçmiş dosyaya kaydedildi: {_logFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Dosya kaydetme hatası: {ex.Message}");
            }
        }

        /// <summary>
        /// Barkod istatistiklerini göster
        /// </summary>
        private static void ShowBarcodeStatistics(Dictionary<string, int> counts, DateTime startTime)
        {
            var duration = DateTime.Now - startTime;
            
            Console.WriteLine($"\n📊 Barkod İstatistikleri:");
            Console.WriteLine($"⏱️  Süre: {duration.TotalSeconds:F1} saniye");
            Console.WriteLine($"📱 Toplam: {counts.Values.Sum()} barkod");
            Console.WriteLine($"📈 Ortalama: {counts.Values.Sum() / duration.TotalSeconds:F1} barkod/saniye");
            Console.WriteLine("\n📋 Tip Dağılımı:");
            
            foreach (var kvp in counts)
            {
                var percentage = (double)kvp.Value / counts.Values.Sum() * 100;
                Console.WriteLine($"   {kvp.Key}: {kvp.Value} adet ({percentage:F1}%)");
            }
        }

        /// <summary>
        /// Konfigürasyon dosyası oluştur
        /// </summary>
        private static async Task CreateConfigurationFile()
        {
            var configContent = @"# CognexBarcodeReader Konfigürasyon Dosyası
Timeout=5000
Baudrate=115200
DebugMode=true
AutoDiscovery=true
ThreadSleep=20";

            await File.WriteAllTextAsync("barcode_config.txt", configContent);
            Console.WriteLine("📄 Konfigürasyon dosyası oluşturuldu: barcode_config.txt");
        }

        /// <summary>
        /// Konfigürasyon dosyasından ayarları yükle
        /// </summary>
        private static BarcodeReaderConfig LoadConfigurationFromFile()
        {
            var config = new BarcodeReaderConfig();
            
            try
            {
                if (File.Exists("barcode_config.txt"))
                {
                    var lines = File.ReadAllLines("barcode_config.txt");
                    
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line))
                            continue;
                            
                        var parts = line.Split('=');
                        if (parts.Length == 2)
                        {
                            var key = parts[0].Trim();
                            var value = parts[1].Trim();
                            
                            switch (key)
                            {
                                case "Timeout":
                                    if (int.TryParse(value, out int timeout))
                                        config.Timeout = timeout;
                                    break;
                                case "Baudrate":
                                    if (int.TryParse(value, out int baudrate))
                                        config.Baudrate = baudrate;
                                    break;
                                case "DebugMode":
                                    if (bool.TryParse(value, out bool debugMode))
                                        config.DebugMode = debugMode;
                                    break;
                                case "AutoDiscovery":
                                    if (bool.TryParse(value, out bool autoDiscovery))
                                        config.AutoDiscovery = autoDiscovery;
                                    break;
                                case "ThreadSleep":
                                    if (int.TryParse(value, out int threadSleep))
                                        config.ThreadSleep = threadSleep;
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️  Konfigürasyon yükleme hatası: {ex.Message}");
            }
            
            return config;
        }

        #endregion
    }
}
