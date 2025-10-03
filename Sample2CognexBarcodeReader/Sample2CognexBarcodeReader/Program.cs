using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample2CognexBarcodeReader
{
    /// <summary>
    /// Ana uygulama sınıfı - Cognex Barkod Okuyucu uygulamasının giriş noktası
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana giriş noktası.
        /// Windows Forms uygulamasını başlatır ve ana formu gösterir.
        /// </summary>
        /// <remarks>
        /// Bu metod uygulamanın başlangıç noktasıdır ve aşağıdaki işlemleri gerçekleştirir:
        /// - Visual Styles'ı etkinleştirir
        /// - Text rendering ayarlarını yapar
        /// - Ana formu (Form1) başlatır ve çalıştırır
        /// </remarks>
        [STAThread]
        static void Main()
        {
            try
            {
                // Visual Styles'ı etkinleştir (modern görünüm için)
                Application.EnableVisualStyles();
                
                // Text rendering ayarlarını yap
                Application.SetCompatibleTextRenderingDefault(false);
                
                // Ana formu başlat ve çalıştır
                Application.Run(new Form1());
            }
            catch (Exception ex)
            {
                // Kritik hata durumunda kullanıcıya bilgi ver
                MessageBox.Show(
                    $"Uygulama başlatılırken kritik bir hata oluştu:\n\n{ex.Message}\n\nUygulama kapatılacak.",
                    "Kritik Hata",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
