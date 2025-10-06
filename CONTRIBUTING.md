# 🤝 Katkıda Bulunma Rehberi - CognexBarcodeReader

Bu projeye katkıda bulunmak istediğiniz için teşekkürler! Bu rehber, projeye nasıl katkıda bulunabileceğinizi açıklar.

## 📋 İçindekiler

- [Kod Katkısı](#-kod-katkısı)
- [Hata Bildirimi](#-hata-bildirimi)
- [Özellik İsteği](#-özellik-isteği)
- [Dokümantasyon](#-dokümantasyon)
- [Geliştirme Ortamı](#-geliştirme-ortamı)
- [Kod Standartları](#-kod-standartları)
- [Test Etme](#-test-etme)
- [Pull Request](#-pull-request)

## 💻 Kod Katkısı

### 1. Fork ve Clone

```bash
# Repository'yi fork edin
# Sonra local'e clone edin
git clone https://github.com/YOUR_USERNAME/CognexBarcodeReader.git
cd CognexBarcodeReader
```

### 2. Branch Oluşturma

```bash
# Yeni feature branch oluşturun
git checkout -b feature/your-feature-name

# Veya bug fix için
git checkout -b bugfix/issue-description
```

### 3. Değişiklikleri Yapma

- Kodunuzu yazın
- Test edin
- Dokümantasyonu güncelleyin
- Commit mesajlarınızı açıklayıcı yazın

### 4. Commit Mesajları

```bash
# İyi commit mesajı örnekleri
git commit -m "feat: Add configuration validation"
git commit -m "fix: Resolve memory leak in image disposal"
git commit -m "docs: Update API documentation"
git commit -m "refactor: Extract image processing logic"
```

**Commit Mesaj Formatı:**
- `feat:` Yeni özellik
- `fix:` Hata düzeltmesi
- `docs:` Dokümantasyon değişikliği
- `style:` Kod formatı (boşluk, noktalama vb.)
- `refactor:` Kod yeniden düzenleme
- `test:` Test ekleme/düzeltme
- `chore:` Build, dependency vb. değişiklikler

## 🐛 Hata Bildirimi

### Hata Bildirirken Dikkat Edilecekler

1. **Açıklayıcı Başlık**: Hatayı özetleyen kısa başlık
2. **Detaylı Açıklama**: Hatayı nasıl oluşturduğunuzu açıklayın
3. **Beklenen Davranış**: Ne olmasını beklediğinizi yazın
4. **Gerçek Davranış**: Ne olduğunu yazın
5. **Sistem Bilgileri**: OS, .NET versiyonu, donanım
6. **Screenshot/Log**: Mümkünse görsel kanıt ekleyin

### Hata Bildirimi Template

```markdown
**Hata Açıklaması**
Kısa ve net hata açıklaması

**Yeniden Üretme Adımları**
1. Şu adımları takip edin...
2. Şu kodu çalıştırın...
3. Hata oluşur

**Beklenen Davranış**
Ne olması gerektiğini açıklayın

**Gerçek Davranış**
Ne olduğunu açıklayın

**Sistem Bilgileri**
- OS: Windows 10/11
- .NET Framework: 4.8
- Cognex SDK: 5.6.3.122
- Donanım: [Cihaz modeli]

**Ek Bilgiler**
Screenshot, log dosyası veya diğer bilgiler
```

## 💡 Özellik İsteği

### Özellik İsteklerinde Dikkat Edilecekler

1. **Açık Başlık**: İstediğiniz özelliği özetleyin
2. **Problem Açıklaması**: Hangi problemi çözeceğini açıklayın
3. **Çözüm Önerisi**: Nasıl implement edilebileceğini düşünün
4. **Alternatifler**: Başka çözüm yolları var mı?
5. **Ek Bilgiler**: Screenshot, mockup vb.

### Özellik İsteği Template

```markdown
**Özellik İsteği**
Kısa ve net özellik açıklaması

**Problem**
Hangi problemi çözeceğini açıklayın

**Önerilen Çözüm**
Nasıl implement edilebileceğini açıklayın

**Alternatifler**
Başka çözüm yolları düşündünüz mü?

**Ek Bilgiler**
Screenshot, mockup veya diğer bilgiler
```

## 📚 Dokümantasyon

### Dokümantasyon Katkısı

- **README.md**: Ana proje dokümantasyonu
- **API_DOCUMENTATION.md**: API referansı
- **CHANGELOG.md**: Değişiklik geçmişi
- **CONTRIBUTING.md**: Bu dosya
- **Kod İçi Yorumlar**: XML documentation

### Dokümantasyon Standartları

- **Markdown** formatı kullanın
- **Türkçe** yazın (İngilizce terimler için parantez kullanın)
- **Emoji** kullanarak görsel zenginlik sağlayın
- **Kod blokları** için syntax highlighting kullanın
- **Tablolar** ile bilgileri düzenleyin

## 🛠️ Geliştirme Ortamı

### Gereksinimler

- **Visual Studio 2019/2022** (Community Edition yeterli)
- **.NET Framework 4.8** SDK
- **Cognex DataMan SDK** (projede dahil)
- **Git** (versiyon kontrolü için)

### Kurulum

1. **Repository'yi clone edin**
```bash
git clone https://github.com/alknbugra/CognexBarcodeReader.git
```

2. **Visual Studio ile açın**
```bash
start Sample2CognexBarcodeReader/Sample2CognexBarcodeReader.sln
```

3. **Dependencies'leri kontrol edin**
   - Cognex SDK DLL'leri `dll/` klasöründe olmalı
   - .NET Framework 4.8 yüklü olmalı

4. **Build edin**
   - Debug modunda F5 ile çalıştırın
   - Release modunda build edin

## 📏 Kod Standartları

### C# Kod Standartları

```csharp
// ✅ İyi örnek
public class BarcodeReaderService : IBarcodeReaderService
{
    private readonly BarcodeReaderConfig _config;
    
    /// <summary>
    /// Servisi başlatır ve cihaz keşfini başlatır
    /// </summary>
    /// <returns>Başlatma işleminin başarı durumu</returns>
    public async Task<bool> StartAsync()
    {
        try
        {
            // Implementation
            return true;
        }
        catch (Exception ex)
        {
            LogError("Servis başlatılırken hata oluştu", ex);
            return false;
        }
    }
}
```

### Naming Conventions

- **Classes**: PascalCase (`BarcodeReaderService`)
- **Methods**: PascalCase (`StartAsync`)
- **Properties**: PascalCase (`IsRunning`)
- **Fields**: camelCase with underscore (`_config`)
- **Constants**: PascalCase (`DefaultTimeout`)
- **Interfaces**: I prefix (`IBarcodeReaderService`)

### Kod Organizasyonu

```csharp
// 1. Using statements
using System;
using System.Threading.Tasks;

// 2. Namespace
namespace Sample2CognexBarcodeReader.Services
{
    // 3. Class declaration
    public class BarcodeReaderService : IBarcodeReaderService
    {
        // 4. Fields
        private readonly BarcodeReaderConfig _config;
        
        // 5. Properties
        public bool IsRunning { get; private set; }
        
        // 6. Constructor
        public BarcodeReaderService(BarcodeReaderConfig config)
        {
            _config = config;
        }
        
        // 7. Public methods
        public async Task<bool> StartAsync()
        {
            // Implementation
        }
        
        // 8. Private methods
        private void LogError(string message, Exception ex = null)
        {
            // Implementation
        }
    }
}
```

## 🧪 Test Etme

### Test Türleri

1. **Unit Tests**: Bireysel metodları test etme
2. **Integration Tests**: Servis entegrasyonlarını test etme
3. **UI Tests**: Form davranışlarını test etme
4. **Manual Tests**: Manuel kullanım testleri

### Test Yazma

```csharp
[Test]
public async Task StartAsync_ShouldReturnTrue_WhenServiceStartsSuccessfully()
{
    // Arrange
    var config = new BarcodeReaderConfig();
    var service = new CognexBarcodeReaderService(config);
    
    // Act
    var result = await service.StartAsync();
    
    // Assert
    Assert.IsTrue(result);
    Assert.IsTrue(service.IsRunning);
}
```

### Test Çalıştırma

```bash
# Unit testleri çalıştır
dotnet test

# Coverage raporu al
dotnet test --collect:"XPlat Code Coverage"
```

## 🔄 Pull Request

### PR Oluştururken

1. **Açıklayıcı Başlık**: Ne yaptığınızı özetleyin
2. **Detaylı Açıklama**: Değişiklikleri açıklayın
3. **Screenshot**: UI değişiklikleri için görsel ekleyin
4. **Test Sonuçları**: Testlerin geçtiğini belirtin
5. **Breaking Changes**: Varsa belirtin

### PR Template

```markdown
## 📝 Değişiklik Açıklaması
Bu PR ne yapıyor?

## 🔧 Değişiklik Türü
- [ ] Bug fix
- [ ] Yeni özellik
- [ ] Breaking change
- [ ] Dokümantasyon güncellemesi
- [ ] Refactoring

## ✅ Test Edildi
- [ ] Unit testler geçiyor
- [ ] Integration testler geçiyor
- [ ] Manuel test yapıldı
- [ ] UI değişiklikleri test edildi

## 📸 Screenshot
Eğer UI değişikliği varsa screenshot ekleyin

## 🔗 İlgili Issue
Fixes #123
```

### PR Review Süreci

1. **Otomatik Kontroller**: CI/CD pipeline'ı geçmeli
2. **Code Review**: En az 1 kişi review etmeli
3. **Test Sonuçları**: Tüm testler geçmeli
4. **Dokümantasyon**: Gerekli dokümantasyon güncellenmeli

## 🏷️ Release Süreci

### Version Naming

- **Major** (2.0.0): Breaking changes
- **Minor** (2.1.0): Yeni özellikler
- **Patch** (2.1.1): Bug fixes

### Release Checklist

- [ ] Tüm testler geçiyor
- [ ] Dokümantasyon güncel
- [ ] CHANGELOG.md güncellenmiş
- [ ] Version numarası artırılmış
- [ ] Release notes hazırlanmış

## 📞 İletişim

- **GitHub Issues**: Hata bildirimi ve özellik istekleri için
- **Discussions**: Genel sorular ve tartışmalar için
- **Email**: [email@example.com] (kritik konular için)

## 🙏 Teşekkürler

Katkıda bulunan herkese teşekkürler! Projeyi daha iyi hale getirmek için çalıştığınız için minnettarız.

---

**Not**: Bu rehber sürekli güncellenmektedir. Önerileriniz varsa lütfen issue açın!








