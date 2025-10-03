# 🎯 Kullanım Örnekleri - CognexBarcodeReader

Bu klasör CognexBarcodeReader projesinin çeşitli kullanım örneklerini içerir.

## 📁 İçerik

### 📚 Temel Örnekler
- **`BasicUsage.cs`** - En basit kullanım örnekleri
- **`AdvancedUsage.cs`** - Gelişmiş kullanım senaryoları

### 🖥️ UI Örnekleri
- **`WindowsFormsExample.cs`** - Windows Forms arayüzü
- **`ConsoleApplication.cs`** - Konsol uygulaması

### 🚀 Demo Projesi
- **`Program.cs`** - Ana demo uygulaması
- **`DemoProject.csproj`** - Demo proje dosyası
- **`App.config`** - Konfigürasyon dosyası

## 🎮 Hızlı Başlangıç

### 1. Demo Uygulamasını Çalıştırın

```bash
# Demo projesini build edin
dotnet build DemoProject.csproj

# Demo uygulamasını çalıştırın
dotnet run --project DemoProject.csproj
```

### 2. Visual Studio ile Açın

```bash
# Visual Studio ile açın
start DemoProject.csproj
```

## 📋 Örnek Kategorileri

### 🔰 Temel Kullanım

#### SimpleExample()
```csharp
// En basit kullanım
var barcodeService = new CognexBarcodeReaderService();
barcodeService.BarcodeRead += (s, e) => Console.WriteLine($"Barkod: {e.BarcodeData}");
await barcodeService.StartAsync();
```

#### CustomConfigurationExample()
```csharp
// Özelleştirilmiş ayarlar
var config = new BarcodeReaderConfig
{
    Timeout = 10000,
    DebugMode = true
};
var barcodeService = new CognexBarcodeReaderService(config);
```

### 🚀 Gelişmiş Kullanım

#### BarcodeHistoryExample()
- Barkod geçmişi tutma
- Dosyaya log yazma
- İstatistik toplama

#### MultipleBarcodeExample()
- Çoklu barkod okuma
- Tip bazında sayım
- Performans metrikleri

#### ErrorHandlingExample()
- Retry mekanizması
- Hata yönetimi
- Graceful degradation

#### ConfigurationFileExample()
- Dosyadan ayar yükleme
- Dinamik konfigürasyon
- Ayarları kaydetme

### 🖥️ UI Örnekleri

#### WindowsFormsExample
- Grafik arayüz
- Gerçek zamanlı görüntüleme
- Barkod geçmişi
- İstatistik paneli

#### ConsoleApplication
- Komut satırı arayüzü
- Menü sistemi
- Konfigürasyon yönetimi
- Test araçları

## 🛠️ Geliştirme

### Proje Yapısı
```
Examples/
├── 📄 BasicUsage.cs              # Temel kullanım örnekleri
├── 📄 AdvancedUsage.cs           # Gelişmiş kullanım örnekleri
├── 📄 WindowsFormsExample.cs     # Windows Forms örneği
├── 📄 ConsoleApplication.cs      # Konsol uygulaması örneği
├── 📄 Program.cs                 # Demo uygulaması ana programı
├── 📄 DemoProject.csproj         # Demo proje dosyası
├── 📄 App.config                 # Konfigürasyon dosyası
└── 📄 README.md                  # Bu dosya
```

### Bağımlılıklar
- **.NET Framework 4.8**
- **Cognex DataMan SDK 5.6.3.122**
- **System.Windows.Forms** (Windows Forms örnekleri için)

### Build Etme
```bash
# Debug modunda build
dotnet build DemoProject.csproj --configuration Debug

# Release modunda build
dotnet build DemoProject.csproj --configuration Release
```

## 📖 Kullanım Senaryoları

### 🏭 Endüstriyel Üretim
```csharp
// Üretim hattında barkod okuma
var config = new BarcodeReaderConfig
{
    Timeout = 3000,  // Hızlı işlem
    DebugMode = false
};
```

### 📦 Depo Yönetimi
```csharp
// Depo envanter takibi
var barcodeService = new CognexBarcodeReaderService();
barcodeService.BarcodeRead += async (s, e) => 
{
    await SaveToDatabase(e.BarcodeData);
    await UpdateInventory(e.BarcodeData);
};
```

### 🔍 Kalite Kontrol
```csharp
// Kalite kontrol süreci
var barcodeService = new CognexBarcodeReaderService();
barcodeService.BarcodeRead += (s, e) => 
{
    ValidateBarcode(e.BarcodeData);
    LogQualityCheck(e.BarcodeData, e.ReadTime);
};
```

## 🎯 Örnek Özellikleri

### ✨ Temel Özellikler
- **Event-driven** mimari
- **Async/await** pattern
- **Exception handling**
- **Resource management**

### 🔧 Gelişmiş Özellikler
- **Configuration management**
- **Logging system**
- **Statistics tracking**
- **File operations**
- **Retry mechanisms**

### 🎨 UI Özellikleri
- **Real-time updates**
- **Image display**
- **History tracking**
- **Status monitoring**
- **User-friendly interface**

## 🚨 Dikkat Edilecekler

### ⚠️ Hardware Gereksinimleri
- **Cognex DM280X** cihazı gerekli
- **Type-C kablosu** bağlantısı
- **Windows 10/11** işletim sistemi

### 🔧 Software Gereksinimleri
- **.NET Framework 4.8** SDK
- **Cognex DataMan SDK** DLL'leri
- **Visual Studio 2019/2022** (geliştirme için)

### 📝 Best Practices
- **Always dispose** servisleri
- **Handle exceptions** properly
- **Use async/await** for long operations
- **Log important events**
- **Test with real hardware**

## 🤝 Katkıda Bulunma

Yeni örnekler eklemek için:

1. **Yeni sınıf oluşturun** (örn: `MyCustomExample.cs`)
2. **XML dokümantasyon** ekleyin
3. **Demo menüsüne** ekleyin
4. **Test edin** gerçek donanımla
5. **Pull request** oluşturun

## 📞 Destek

- **GitHub Issues**: Hata bildirimi
- **Discussions**: Sorular ve öneriler
- **Email**: [email@example.com]

---

**Not**: Bu örnekler eğitim amaçlıdır. Üretim ortamında kullanmadan önce kapsamlı test yapın.
