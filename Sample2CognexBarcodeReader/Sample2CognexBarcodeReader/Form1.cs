using Cognex.DataMan.SDK;
using Cognex.DataMan.SDK.Discovery;
using Cognex.DataMan.SDK.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using ConnectionState = Cognex.DataMan.SDK.ConnectionState;

namespace Sample2CognexBarcodeReader
{
    public partial class Form1 : Form
    {

        private ResultCollector _results;
        private SynchronizationContext _syncContext = null;
        private SerSystemDiscoverer _serSystemDiscoverer = null;
        private ISystemConnector _connector = null;
        private DataManSystem _system = null;
        private object _currentResultInfoSyncLock = new object();


        public Form1()
        {
            InitializeComponent();


            // The SDK may fire events from arbitrary thread context. Therefore if you want to change
            // the state of controls or windows from any of the SDK' events, you have to use this
            // synchronization context to execute the event handler code on the main GUI thread.
            _syncContext = WindowsFormsSynchronizationContext.Current;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                // Create discoverers to discover ethernet and serial port systems.
                _serSystemDiscoverer = new SerSystemDiscoverer();

                // Subscribe to the system discoved event.
                _serSystemDiscoverer.SystemDiscovered += new SerSystemDiscoverer.SystemDiscoveredHandler(OnSerSystemDiscovered);

                // Ask the discoverers to start discovering systems.
                _serSystemDiscoverer.Discover();
            }
            catch (Exception ex)
            {
                CleanupConnection();

                Console.WriteLine("Failed to connect: " + ex.ToString());
            }
        }

        private void OnSystemConnected(object sender, EventArgs args)
        {
            _syncContext.Post(
                delegate
                {
                    Console.WriteLine("System connected");
                },
                null);
        }
        private void OnSystemDisconnected(object sender, EventArgs args)
        {
            _syncContext.Post(
                delegate
                {
                    Console.WriteLine("System disconnected");
                },
                null);
        }

        private void Results_ComplexResultCompleted(object sender, ComplexResult e)
        {
            _syncContext.Post(
                delegate
                {
                    ShowResult(e);
                },
                null);
        }
        private void ShowResult(ComplexResult complexResult)
        {
            List<Image> images = new List<Image>();
            List<string> image_graphics = new List<string>();
            string read_result = null;
            int result_id = -1;
            ResultTypes collected_results = ResultTypes.None;

            // Take a reference or copy values from the locked result info object. This is done
            // so that the lock is used only for a short period of time.
            lock (_currentResultInfoSyncLock)
            {
                foreach (var simple_result in complexResult.SimpleResults)
                {
                    collected_results |= simple_result.Id.Type;

                    switch (simple_result.Id.Type)
                    {
                        case ResultTypes.Image:
                            Image image = ImageArrivedEventArgs.GetImageFromImageBytes(simple_result.Data);
                            if (image != null)
                                images.Add(image);
                            break;

                        case ResultTypes.ImageGraphics:
                            image_graphics.Add(simple_result.GetDataAsString());
                            break;

                        case ResultTypes.ReadXml:
                            read_result = GetReadStringFromResultXml(simple_result.GetDataAsString());
                            result_id = simple_result.Id.Id;
                            break;

                        case ResultTypes.ReadString:
                            read_result = simple_result.GetDataAsString();
                            result_id = simple_result.Id.Id;
                            break;
                    }
                }
            }

            Console.WriteLine(string.Format("Complex result arrived: resultId = {0}, read result = {1}", result_id, read_result));

            if (images.Count > 0)
            {
                Image first_image = images[0];

                Size image_size = Gui.FitImageInControl(first_image.Size, picResultImage.Size);
                Image fitted_image = Gui.ResizeImageToBitmap(first_image, image_size);

                if (image_graphics.Count > 0)
                {
                    using (Graphics g = Graphics.FromImage(fitted_image))
                    {
                        foreach (var graphics in image_graphics)
                        {
                            ResultGraphics rg = GraphicsResultParser.Parse(graphics, new Rectangle(0, 0, image_size.Width, image_size.Height));
                            ResultGraphicsRenderer.PaintResults(g, rg);
                        }
                    }
                }

                if (picResultImage.Image != null)
                {
                    var image = picResultImage.Image;
                    picResultImage.Image = null;
                    image.Dispose();
                }

                picResultImage.Image = fitted_image;
                picResultImage.Invalidate();
            }

            if (read_result != null)
                lbReadString.Text = read_result;
        }
        private string GetReadStringFromResultXml(string resultXml)
        {
            try
            {
                XmlDocument doc = new XmlDocument();

                doc.LoadXml(resultXml);

                XmlNode full_string_node = doc.SelectSingleNode("result/general/full_string");

                if (full_string_node != null && _system != null && _system.State == ConnectionState.Connected)
                {
                    XmlAttribute encoding = full_string_node.Attributes["encoding"];
                    if (encoding != null && encoding.InnerText == "base64")
                    {
                        if (!string.IsNullOrEmpty(full_string_node.InnerText))
                        {
                            byte[] code = Convert.FromBase64String(full_string_node.InnerText);
                            return _system.Encoding.GetString(code, 0, code.Length);
                        }
                        else
                        {
                            return "";
                        }
                    }

                    return full_string_node.InnerText;
                }
            }
            catch
            {
            }

            return "";
        }


        private void Results_SimpleResultDropped(object sender, SimpleResult e)
        {
            _syncContext.Post(
                delegate
                {
                    ReportDroppedResult(e);
                },
                null);
        }
        private void ReportDroppedResult(SimpleResult result)
        {
            Console.WriteLine(string.Format("Partial result dropped: {0}, id={1}", result.Id.Type.ToString(), result.Id.Id));
        }




        private void OnSerSystemDiscovered(SerSystemDiscoverer.SystemInfo systemInfo)
        {

            Console.WriteLine(string.Format("Cihaz bulundu: {0}, id={1}", systemInfo.Name, systemInfo.PortName));

            SerSystemConnector conn = new SerSystemConnector(systemInfo.PortName, systemInfo.Baudrate);

            _connector = conn;

            _system = new DataManSystem(_connector);
            _system.DefaultTimeout = 5000;

            // Subscribe to events that are signalled when the system is connected / disconnected.
            _system.SystemConnected += new SystemConnectedHandler(OnSystemConnected);
            _system.SystemDisconnected += new SystemDisconnectedHandler(OnSystemDisconnected);

            // Subscribe to events that are signalled when the device sends auto-responses.
            ResultTypes requested_result_types = ResultTypes.ReadXml | ResultTypes.Image | ResultTypes.ImageGraphics;
            _results = new ResultCollector(_system, requested_result_types);
            _results.ComplexResultCompleted += Results_ComplexResultCompleted;
            _results.SimpleResultDropped += Results_SimpleResultDropped;

            _system.Connect();

            try
            {
                _system.SetResultTypes(requested_result_types);
            }
            catch
            {
            }

            while (true)
            {
                Thread.Sleep(20);
            }

        }

        private void CleanupConnection()
        {
            if (null != _system)
            {
                _system.SystemConnected -= OnSystemConnected;
                _system.SystemDisconnected -= OnSystemDisconnected;
            }

            _connector = null;
            _system = null;
        }


    }
}
