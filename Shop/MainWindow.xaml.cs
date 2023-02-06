using Shop.Concrete;
using Shop.Network;
using Shop.User_Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace Shop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int SelectItemIndex { get; set; } = 0;
        string[] Commands { get; set; } 
        public MainWindow()
        {
            InitializeComponent();
            //Combo_list.Items.Add("ALL Category");
            //Combo_list.SelectedIndex= 0;
            Connection_Method();
           
        }
        private static readonly Socket ClientSocket = new Socket
           (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private void RequestLoop()
        {
            var receiver = Task.Run(() =>
            {
                while (true)
                    ReceiveResponse();
            });

            //Task.WaitAll(receiver);
        }

        private void AddItemList(List<Product> Data)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                list.Items.Clear();
                if (SelectItemIndex!=0)
                {
                    foreach (var item in Data)
                    {
                        if (item.CategoryId==SelectItemIndex)
                        {
                            list.Items.Add(new UC(item));
                        }
                    }
                }
                else
                {
                    foreach (var item in Data)
                    {
                        list.Items.Add(new UC(item));
                    }
                }
            });
        }

        private void ReceiveResponse()
        {
            var buffer = new byte[100000];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
                string text = Encoding.ASCII.GetString(data);
                //MessageBox.Show(text);
                var data1 = JsonSerializer.Deserialize<List<Product>>(text);
            AddItemList(data1);
        }

        private void Connection_Method()
        {
            Task.Run(() =>
            {
                while (!ClientSocket.Connected)
                {
                    try
                    {
                        ClientSocket.Connect(Connection_Key.IPADDRESS, Connection_Key.PORT);
                    }
                    catch (SocketException) { continue; }
                }
                string Message = Server_Send();
                Commands = Message.Split('\n');
              
                if (Commands.Length>0)
                {
                    foreach (var item in Commands)
                    {
                        if (item.IndexOf("Category")!=-1&& item.LastIndexOf("GetList")!=1)
                        {
                            SendString(item);
                            ParseForView(Server_Send());
                            //MessageBox.Show(Message);
                            string str = List_Search(Commands, "Product", "GetAll");
                            //MessageBox.Show(str);
                            if (str != string.Empty)
                            {
                                SendString(str);
                            }
                            RequestLoop();
                            return;
                        }
                    }
                    
                }
            });

        }

        private string List_Search(string[] TList , string data1,string data2)
        {
            foreach (var item in TList)
            {
                if (item.IndexOf(data1) != -1 && item.LastIndexOf(data2) != -1)
                {
                    return item;
                }
            }
            return string.Empty;
        }
        private string Server_Send()
        {
            var buffer = new byte[2048];
            int received = ClientSocket.Receive(buffer, SocketFlags.None);
            if (received == 0) return null;
            var data = new byte[received];
            Array.Copy(buffer, data, received);
            string text = Encoding.ASCII.GetString(data);
            return text;
            

        }
        private void SendString(string text)
        {
            var result = text.Remove(text.Length - 1, 1);
            byte[] buffer = Encoding.ASCII.GetBytes(result);
            ClientSocket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        private void ParseForView(string text)
        {
            var jsonstr = JsonSerializer.Deserialize<List<Category>>(text);
            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var item in jsonstr)
                {
                    Combo_list.Items.Add(item.ToString());
                }

            });
        }


        private void Combo_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Combo_list.SelectedIndex!=0)
            {
                var item = Combo_list.SelectedItem.ToString();
                var commands = item.Split(' ');
                SelectItemIndex = Convert.ToInt32(commands[0]);
                string str = List_Search(Commands, "Product", "GetAll");
                if (str != string.Empty)
                {
                    SendString(str);
                }
            }
        }
    }
}
