# 📚 API Dokümantasyonu - CognexBarcodeReader

Bu dokümantasyon CognexBarcodeReader projesinin API'sini detaylı olarak açıklar.

## 🏗️ Mimari Genel Bakış

Proje **Clean Architecture** prensiplerine uygun olarak tasarlanmıştır:

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Presentation  │    │    Business     │    │   Data Access   │
│     Layer       │    │     Layer       │    │     Layer       │
├─────────────────┤    ├─────────────────┤    ├─────────────────┤
│     Form1       │◄──►│ IBarcodeReader  │◄──►│ Cognex SDK      │
│   (UI Layer)    │    │    Service      │    │   (Hardware)    │
└─────────────────┘    └─────────────────┘    └─────────────────┘
```

## 🔌 Interface'ler

### IBarcodeReaderService

Barkod okuyucu servisi için temel interface.

```csharp
public interface IBarcodeReaderService
{
    // Events
    event EventHandler<EventArgs> DeviceConnected;
    event EventHandler<EventArgs> DeviceDisconnected;
    event EventHandler<BarcodeReadEventArgs> BarcodeRead;

    // Methods
    Task<bool> StartAsync();
    Task<bool> StopAsync();

    // Properties
    bool IsRunning { get; }
    bool IsConnected { get; }
}
```

#### Events

| Event | Açıklama | Parametreler |
|-------|----------|--------------|
| `DeviceConnected` | Cihaz bağlandığında tetiklenir | `EventArgs` |
| `DeviceDisconnected` | Cihaz bağlantısı kesildiğinde tetiklenir | `EventArgs` |
| `BarcodeRead` | Barkod okunduğunda tetiklenir | `BarcodeReadEventArgs` |

#### Methods

| Method | Açıklama | Dönüş Değeri |
|--------|----------|--------------|
| `StartAsync()` | Servisi başlatır ve cihaz keşfini başlatır | `Task<bool>` |
| `StopAsync()` | Servisi durdurur ve kaynakları temizler | `Task<bool>` |

#### Properties

| Property | Açıklama | Tip |
|----------|----------|-----|
| `IsRunning` | Servisin çalışıp çalışmadığını kontrol eder | `bool` |
| `IsConnected` | Cihazın bağlı olup olmadığını kontrol eder | `bool` |

## 📦 Model Sınıfları

### BarcodeReadEventArgs

Barkod okuma eventi için argüman sınıfı.

```csharp
public class BarcodeReadEventArgs : EventArgs
{
    public string BarcodeData { get; }      // Okunan barkod verisi
    public Image BarcodeImage { get; }      // Barkod görüntüsü
    public int ResultId { get; }            // Sonuç ID'si
    public DateTime ReadTime { get; }       // Okuma zamanı
    public string BarcodeType { get; }      // Barkod tipi
}
```

### BarcodeReaderConfig

Barkod okuyucu konfigürasyon sınıfı.

```csharp
public class BarcodeReaderConfig
{
    // Constants
    public const int DefaultTimeout = 5000;
    public const int DefaultBaudrate = 115200;
    public const int DefaultThreadSleep = 20;

    // Properties
    public int Timeout { get; set; }        // Sistem timeout değeri
    public int Baudrate { get; set; }       // Seri port baudrate değeri
    public int ThreadSleep { get; set; }    // Thread sleep süresi
    public bool AutoDiscovery { get; set; } // Otomatik keşif yapılıp yapılmayacağı
    public bool DebugMode { get; set; }     // Debug modu aktif mi
}
```

## 🛠️ Servis Sınıfları

### CognexBarcodeReaderService

Cognex barkod okuyucu servisi implementasyonu.

#### Constructor

```csharp
public CognexBarcodeReaderService(BarcodeReaderConfig config = null)
```

**Parametreler:**
- `config`: Konfigürasyon ayarları (opsiyonel, null ise varsayılan değerler kullanılır)

#### Public Methods

| Method | Açıklama | Parametreler | Dönüş Değeri |
|--------|----------|--------------|--------------|
| `StartAsync()` | Servisi başlatır | Yok | `Task<bool>` |
| `StopAsync()` | Servisi durdurur | Yok | `Task<bool>` |

#### Public Properties

| Property | Açıklama | Tip |
|----------|----------|-----|
| `IsRunning` | Servisin çalışıp çalışmadığını kontrol eder | `bool` |
| `IsConnected` | Cihazın bağlı olup olmadığını kontrol eder | `bool` |

#### Events

| Event | Açıklama | Parametreler |
|-------|----------|--------------|
| `DeviceConnected` | Cihaz bağlandığında tetiklenir | `EventArgs` |
| `DeviceDisconnected` | Cihaz bağlantısı kesildiğinde tetiklenir | `EventArgs` |
| `BarcodeRead` | Barkod okunduğunda tetiklenir | `BarcodeReadEventArgs` |

## 🎨 UI Sınıfları

### Form1

Ana uygulama formu - Cognex barkod okuyucu arayüzü.

#### Public Methods

| Method | Açıklama | Parametreler | Dönüş Değeri |
|--------|----------|--------------|--------------|
| `IsServiceRunning` | Servis durumunu kontrol eder | Yok | `bool` |
| `IsDeviceConnected` | Cihaz bağlantı durumunu kontrol eder | Yok | `bool` |

#### Events

| Event | Açıklama | Parametreler |
|-------|----------|--------------|
| `Form1_Load` | Form yüklendiğinde tetiklenir | `object sender, EventArgs e` |

## 🔧 Utility Sınıfları

### ImageHelper

Görüntü işleme yardımcı sınıfı.

#### Static Methods

| Method | Açıklama | Parametreler | Dönüş Değeri |
|--------|----------|--------------|--------------|
| `FitImageInControl` | Görüntüyü kontrol boyutuna uygun şekilde yeniden boyutlandırır | `Size originalSize, Size controlSize` | `Size` |
| `ResizeImageToBitmap` | Görüntüyü belirtilen boyuta yeniden boyutlandırır | `Image originalImage, Size newSize` | `Bitmap` |
| `ApplyGraphicsOverlay` | Graphics overlay'i görüntüye uygular | `Bitmap image, string[] graphicsData, Size imageSize` | `Bitmap` |
| `SafeDisposeImage` | Görüntüyü güvenli şekilde dispose eder | `Image image` | `void` |

## 📋 Kullanım Örnekleri

### Temel Kullanım

```csharp
// Konfigürasyon oluştur
var config = new BarcodeReaderConfig
{
    Timeout = 5000,
    Baudrate = 115200,
    DebugMode = true
};

// Servis oluştur
var barcodeService = new CognexBarcodeReaderService(config);

// Event'leri bağla
barcodeService.DeviceConnected += (s, e) => Console.WriteLine("Cihaz bağlandı!");
barcodeService.BarcodeRead += (s, e) => Console.WriteLine($"Barkod: {e.BarcodeData}");

// Servisi başlat
await barcodeService.StartAsync();
```

### Form ile Kullanım

```csharp
public partial class Form1 : Form
{
    private IBarcodeReaderService _barcodeService;

    private async void Form1_Load(object sender, EventArgs e)
    {
        _barcodeService = new CognexBarcodeReaderService();
        _barcodeService.BarcodeRead += OnBarcodeRead;
        await _barcodeService.StartAsync();
    }

    private void OnBarcodeRead(object sender, BarcodeReadEventArgs e)
    {
        // UI'yi güncelle
        this.Invoke(new Action(() =>
        {
            label1.Text = e.BarcodeData;
            pictureBox1.Image = e.BarcodeImage;
        }));
    }
}
```

## ⚠️ Hata Yönetimi

### Yaygın Hatalar ve Çözümleri

| Hata | Sebep | Çözüm |
|------|-------|-------|
| `DeviceNotFoundException` | Cihaz bulunamadı | Type-C kablosunu kontrol edin |
| `TimeoutException` | Bağlantı zaman aşımı | Timeout değerini artırın |
| `SDKNotLoadedException` | SDK yüklenemedi | DLL dosyalarını kontrol edin |

### Exception Handling

```csharp
try
{
    await barcodeService.StartAsync();
}
catch (DeviceNotFoundException ex)
{
    MessageBox.Show("Cihaz bulunamadı. Lütfen bağlantıyı kontrol edin.");
}
catch (TimeoutException ex)
{
    MessageBox.Show("Bağlantı zaman aşımına uğradı.");
}
catch (Exception ex)
{
    MessageBox.Show($"Beklenmeyen hata: {ex.Message}");
}
```

## 🔍 Debug ve Logging

### Debug Modu

```csharp
var config = new BarcodeReaderConfig
{
    DebugMode = true  // Console'a detaylı log çıktısı
};
```

### Log Seviyeleri

- **INFO**: Genel bilgi mesajları
- **WARNING**: Uyarı mesajları
- **ERROR**: Hata mesajları

## 📊 Performans İpuçları

1. **Memory Management**: Görüntüleri kullanımdan sonra dispose edin
2. **Thread Safety**: UI güncellemelerini Invoke ile yapın
3. **Resource Cleanup**: Servisi kullanımdan sonra StopAsync() çağırın
4. **Exception Handling**: Tüm async metodları try-catch ile sarmalayın

## 🔄 Lifecycle

```
1. Constructor → 2. StartAsync() → 3. Device Discovery → 4. Connect → 5. Barcode Reading → 6. StopAsync() → 7. Dispose
```

Bu dokümantasyon projenin API'sini kapsamlı olarak açıklar. Daha fazla bilgi için kaynak kodları inceleyebilirsiniz.
