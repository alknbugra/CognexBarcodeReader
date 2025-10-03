# 🔍 CognexBarcodeReader

[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-.NET%20Framework%204.8-blueviolet.svg)](https://dotnet.microsoft.com/download/dotnet-framework)
[![Cognex SDK](https://img.shields.io/badge/Cognex%20SDK-5.6.3.122-orange.svg)](https://www.cognex.com/)
[![Build Status](https://img.shields.io/badge/Build-Passing-brightgreen.svg)](https://github.com/alknbugra/CognexBarcodeReader)
[![Last Commit](https://img.shields.io/github/last-commit/alknbugra/CognexBarcodeReader?color=orange)](https://github.com/alknbugra/CognexBarcodeReader/commits/main)
[![Repo Size](https://img.shields.io/github/repo-size/alknbugra/CognexBarcodeReader)](https://github.com/alknbugra/CognexBarcodeReader)

> **Endüstriyel barkod okuyucu uygulaması** - Cognex DM280X barkod okuyucu ile Type-C kablosu üzerinden seri port iletişimi sağlayan profesyonel C# uygulaması.

## 📋 İçindekiler

- [Özellikler](#-özellikler)
- [Gereksinimler](#-gereksinimler)
- [Kurulum](#-kurulum)
- [Kullanım](#-kullanım)
- [Proje Yapısı](#-proje-yapısı)
- [API Referansı](#-api-referansı)
- [Troubleshooting](#-troubleshooting)
- [Katkıda Bulunma](#-katkıda-bulunma)
- [Lisans](#-lisans)

## ✨ Özellikler

### 🎯 Temel Özellikler
- **🔗 Otomatik Cihaz Keşfi** - Seri port üzerinden Cognex cihazlarını otomatik bulma
- **📱 Gerçek Zamanlı Barkod Okuma** - Anlık barkod okuma ve görüntüleme
- **🖼️ Görsel Arayüz** - Okunan barkod görüntüsü ile birlikte sonuç gösterimi
- **⚡ Thread-Safe İşlemler** - UI thread'i bloklamadan güvenli veri işleme
- **🔧 Esnek Konfigürasyon** - Timeout, baudrate ve diğer parametreler

### 🛠️ Teknik Özellikler
- **Cognex DataMan SDK 5.6.3.122** entegrasyonu
- **Seri Port İletişimi** (Type-C kablosu desteği)
- **XML/Base64** veri parsing
- **Görsel Graphics** overlay desteği
- **Multi-threading** mimarisi

## 🔧 Gereksinimler

### Donanım Gereksinimleri
- **Cognex DM280X** barkod okuyucu cihazı
- **Type-C kablosu** (seri port bağlantısı için)
- **Windows 10/11** (64-bit önerilen)
- **Minimum 4GB RAM**
- **500MB boş disk alanı**

### Yazılım Gereksinimleri
- **.NET Framework 4.8** veya üzeri
- **Visual Studio 2019/2022** (geliştirme için)
- **Cognex DataMan SDK** (projede dahil)

## 🚀 Kurulum

### 1. Repository'yi Klonlayın
```bash
git clone https://github.com/alknbugra/CognexBarcodeReader.git
cd CognexBarcodeReader
```

### 2. Visual Studio ile Açın
```bash
# Visual Studio ile
start Sample2CognexBarcodeReader/Sample2CognexBarcodeReader.sln

# Veya dotnet CLI ile
dotnet restore Sample2CognexBarcodeReader/Sample2CognexBarcodeReader.sln
```

### 3. Bağımlılıkları Kontrol Edin
Proje klasöründe `dll/` dizininde gerekli Cognex SDK dosyaları bulunmaktadır:
- `Cognex.DataMan.SDK.PC.dll`
- `Cognex.DataMan.SDK.Discovery.PC.dll`
- `Cognex.DataMan.SDK.Utils.PC.dll`

### 4. Uygulamayı Çalıştırın
```bash
# Visual Studio'dan F5 ile
# Veya build edip exe'yi çalıştırın
```

## 📖 Kullanım

### Temel Kullanım
1. **Cognex DM280X cihazını** Type-C kablosu ile bilgisayara bağlayın
2. **Uygulamayı başlatın** - cihaz otomatik olarak keşfedilecektir
3. **Barkod okutun** - sonuçlar anlık olarak görüntülenecektir

### Arayüz Bileşenleri
- **📷 Görüntü Alanı** - Okunan barkod görüntüsü
- **📝 Sonuç Metni** - Okunan barkod verisi
- **🔍 Graphics Overlay** - Barkod sınırları ve işaretçiler

### Desteklenen Barkod Formatları
- **QR Code**
- **Data Matrix**
- **Code 128**
- **Code 39**
- **EAN/UPC**
- Ve diğer Cognex destekli formatlar

## 📁 Proje Yapısı

```
CognexBarcodeReader/
├── 📁 Sample2CognexBarcodeReader/          # Ana proje klasörü
│   ├── 📁 Sample2CognexBarcodeReader/      # Kaynak kodlar
│   │   ├── 📁 dll/                         # Cognex SDK dosyaları
│   │   ├── 📁 Properties/                  # Proje özellikleri
│   │   ├── 📄 Form1.cs                     # Ana form sınıfı
│   │   ├── 📄 Form1.Designer.cs            # UI tasarım dosyası
│   │   ├── 📄 Program.cs                   # Uygulama giriş noktası
│   │   └── 📄 *.csproj                     # Proje dosyası
│   └── 📄 Sample2CognexBarcodeReader.sln   # Solution dosyası
├── 📁 images/                              # Ekran görüntüleri
├── 📄 README.md                            # Bu dosya
├── 📄 LICENSE                              # MIT lisansı
└── 📄 .gitignore                           # Git ignore kuralları
```

## 🔌 API Referansı

### Ana Sınıflar

#### `Form1` - Ana Uygulama Sınıfı
```csharp
public partial class Form1 : Form
{
    private ResultCollector _results;                    // Sonuç toplayıcı
    private ISystemConnector _connector;                 // Sistem bağlantısı
    private DataManSystem _system;                       // Cognex sistem nesnesi
    private SerSystemDiscoverer _serSystemDiscoverer;    // Seri port keşif
}
```

#### Temel Metodlar
```csharp
// Cihaz keşfi başlatma
private void Form1_Load(object sender, EventArgs e)

// Barkod sonucu işleme
private void ShowResult(ComplexResult complexResult)

// XML sonuç parsing
private string GetReadStringFromResultXml(string resultXml)
```

### Event Handlers
- `OnSystemConnected` - Cihaz bağlantı eventi
- `OnSystemDisconnected` - Cihaz bağlantı kesilme eventi
- `Results_ComplexResultCompleted` - Barkod okuma tamamlama eventi

## 🔧 Troubleshooting

### Yaygın Sorunlar

#### ❌ "Cihaz bulunamadı" Hatası
**Çözüm:**
1. Type-C kablosunun doğru bağlandığından emin olun
2. Cihazın güç aldığından kontrol edin
3. Seri port sürücülerinin yüklü olduğunu kontrol edin
4. Farklı bir USB portu deneyin

#### ❌ "SDK yüklenemedi" Hatası
**Çözüm:**
1. `dll/` klasöründeki dosyaların mevcut olduğunu kontrol edin
2. .NET Framework 4.8'in yüklü olduğunu kontrol edin
3. Visual C++ Redistributable'ı yükleyin

#### ❌ "Timeout" Hatası
**Çözüm:**
1. Cihazın baudrate ayarlarını kontrol edin
2. Kablo kalitesini kontrol edin
3. Timeout değerini artırın (kodda 5000ms)

### Debug Modu
```csharp
// Console.WriteLine çıktılarını görmek için
// Visual Studio Output penceresini açın
```

## 🤝 Katkıda Bulunma

Bu projeye katkıda bulunmak için:

1. **Fork** edin
2. **Feature branch** oluşturun (`git checkout -b feature/AmazingFeature`)
3. **Commit** edin (`git commit -m 'Add some AmazingFeature'`)
4. **Push** edin (`git push origin feature/AmazingFeature`)
5. **Pull Request** oluşturun

### Geliştirme Kuralları
- **C# Coding Standards** kullanın
- **XML Documentation** ekleyin
- **Unit Test** yazın (mümkünse)
- **README** güncelleyin

## 📄 Lisans

Bu proje [MIT Lisansı](LICENSE) altında lisanslanmıştır.

## 👨‍💻 Geliştirici

**Buğra Alkın** - [@alknbugra](https://github.com/alknbugra)

- 🔗 **LinkedIn:** [Profiliniz]
- 📧 **Email:** [email@example.com]
- 🌐 **Website:** [websiteniz.com]

## 🙏 Teşekkürler

- **Cognex Corporation** - DataMan SDK için
- **Microsoft** - .NET Framework için
- **Açık kaynak topluluğu** - İlham ve destek için

---

<div align="center">

**⭐ Bu projeyi beğendiyseniz yıldız vermeyi unutmayın!**

[![GitHub stars](https://img.shields.io/github/stars/alknbugra/CognexBarcodeReader?style=social)](https://github.com/alknbugra/CognexBarcodeReader/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/alknbugra/CognexBarcodeReader?style=social)](https://github.com/alknbugra/CognexBarcodeReader/network)

</div>