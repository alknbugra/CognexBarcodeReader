# CognexBarcodeReader

**Cognex DM280X barkod okuyucu ile Type-C kablosu üzerinden seri port iletiimi sağlayan örnek uygulama.**

#Özellikler

- Barkod okuma
- Okunan barkod verilerini ekran görüntüsü ile birlikte gösterme

# Görsel
![Barkod Okuma](images/DM280X.jpg)
![Barkod Okuma](images/Running.gif)

# Kurulum

```bash
# Repo'yu klonla
git clone https://github.com/alknbugra/CognexBarcodeReader.git

# Dizine gir
cd CognexBarcodeReader

# Gerekli bağımlılıkları yükle
- (C# projesi olduğundan, örnek Visual Studio ortamında açılabilir veya dotnet CLI ile)
- dotnet restore veya Visual Studio ile sln dosyasını aç

# Uygulamayı çalıştır
- Visual Studio üzerinden F5 ile ya da dotnet run komutu (varsa) ile çalıştır

# Kullanım
- Uygulama ekranında okunan barkod verileri ekranda görüntülenir
- (Kod snippet'inle ilgili örnek ekleyebilirsin)


![Build Status](https://img.shields.io/github/actions/workflow/status/alknbugra/CognexBarcodeReader/build.yml?branch=main)
![License](https://img.shields.io/github/license/alknbugra/CognexBarcodeReader)
![Release](https://img.shields.io/github/v/release/alknbugra/CognexBarcodeReader)
![Issues](https://img.shields.io/github/issues/alknbugra/CognexBarcodeReader)
