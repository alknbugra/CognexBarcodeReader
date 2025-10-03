using System;
using System.Drawing;

namespace Sample2CognexBarcodeReader.Models
{
    /// <summary>
    /// Barkod okuma eventi için argüman sınıfı
    /// </summary>
    public class BarcodeReadEventArgs : EventArgs
    {
        /// <summary>
        /// Okunan barkod verisi
        /// </summary>
        public string BarcodeData { get; }

        /// <summary>
        /// Barkod görüntüsü
        /// </summary>
        public Image BarcodeImage { get; }

        /// <summary>
        /// Sonuç ID'si
        /// </summary>
        public int ResultId { get; }

        /// <summary>
        /// Okuma zamanı
        /// </summary>
        public DateTime ReadTime { get; }

        /// <summary>
        /// Barkod tipi
        /// </summary>
        public string BarcodeType { get; }

        /// <summary>
        /// Yeni BarcodeReadEventArgs örneği oluşturur
        /// </summary>
        /// <param name="barcodeData">Okunan barkod verisi</param>
        /// <param name="barcodeImage">Barkod görüntüsü</param>
        /// <param name="resultId">Sonuç ID'si</param>
        /// <param name="barcodeType">Barkod tipi</param>
        public BarcodeReadEventArgs(string barcodeData, Image barcodeImage, int resultId, string barcodeType = "Unknown")
        {
            BarcodeData = barcodeData ?? string.Empty;
            BarcodeImage = barcodeImage;
            ResultId = resultId;
            BarcodeType = barcodeType;
            ReadTime = DateTime.Now;
        }
    }
}
