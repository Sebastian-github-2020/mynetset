using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using Zaks.Tools;
namespace 设置dns
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {

        MainViewModel viewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
            // 绑定viewmodel
            this.DataContext = viewModel;
        }

        private void adaptersSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox a = (ComboBox)sender;
            //Console.WriteLine(a.SelectedValue.ToString());
            string name = a.SelectedValue.ToString();
            this.viewModel.SelectedAdapterName = name;
            // 通过网卡名称查询网卡信息         
            // 赋值给viewmodel
            this.viewModel.AdapterInfos = NetTools.ReadAdapterInfo(name);
        }


        // 快速选择dns
        private void QuickSetupDns(object sender, RoutedEventArgs e)
        {

            Button btn = sender as Button; // 在高版本c#中需要异常处理 as转换失败会返回null

            // 读取tooltip里面的dns
            string[] dns = btn.ToolTip.ToString().Trim().Split(new string[] { "和" }, 2, StringSplitOptions.RemoveEmptyEntries);
            // 去空格
            dns[0] = dns[0].Trim();
            dns[1] = dns[1].Trim();
            // 填充textbox
            this.mainDnsTextBox.Text = dns[0];
            this.alternateDnsTextBox.Text = dns[1];
            Console.WriteLine(btn.ToolTip.ToString());
        }
        // 应用所有修改
        private void submit_Click(object sender, RoutedEventArgs e)
        {
            // 1. 判定是否选中网卡
            if(this.viewModel.SelectedAdapterName == null)
            {
                MessageBox.Show("请先选择要设置的网卡", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // 2. 判定IP是否设置,为空或者未设置 则不进行ip设置
            string ip = this.ipTextBox.Text;
            if(!string.IsNullOrEmpty(ip))
            {
                bool res = NetTools.IsIp(ip);
                if(!res)
                {
                    MessageBox.Show("输入的IP不合规", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                } else
                {
                    // 执行设置
                    bool ipRes = NetTools.SetIp(ip, this.viewModel.SelectedAdapterName);
                    if(!ipRes)
                    {
                        MessageBox.Show("IP设置失败，请确保是管理员模式运行", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }
            }
            // 3. 判定dns是否设置
            string primaryDns = this.mainDnsTextBox.Text;
            string alternateDns = this.alternateDnsTextBox.Text;
            if(string.IsNullOrEmpty(primaryDns) != true && string.IsNullOrEmpty(alternateDns) != true)
            {
                // 执行设置dns
                //1. 校验ip
                if(NetTools.IsIp(primaryDns) == true && NetTools.IsIp(alternateDns) == true)
                {
                    // 执行设置
                    bool ares = NetTools.SetDnsPrimary(alternateDns, this.viewModel.SelectedAdapterName);
                    bool pRes = NetTools.SetDnsAlternate(primaryDns, this.viewModel.SelectedAdapterName);
                    if(!ares || !pRes)
                    {
                        MessageBox.Show("Dns设置失败，请确保是管理员模式运行", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                } else
                {
                    MessageBox.Show("请检查DNS是否输入正确", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

            }

            MessageBox.Show("设置成功，如果未生效请确保是管理员模式运行", "温馨提示", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        // 重新获取当前网卡信息
        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            if(this.viewModel.SelectedAdapterName != null)
            {
                this.viewModel.AdapterInfos = NetTools.ReadAdapterInfo(this.viewModel.SelectedAdapterName);
            }
        }
    }
}
