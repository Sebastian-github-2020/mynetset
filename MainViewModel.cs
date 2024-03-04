using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Zaks.Tools;

namespace 设置dns
{
    public class MainViewModel : ViewModelBase
    {
        // 网卡列表
        private ObservableCollection<string> adpaterNames;

        public ObservableCollection<string> AdpaterNames
        {
            get { return adpaterNames; }
            set { adpaterNames = value; OnPropertyChanged(); }
        }
        // 网卡的信息 
        private string adapterInfos;

        public string AdapterInfos
        {
            get { return adapterInfos; }
            set { adapterInfos = value; OnPropertyChanged(); }
        }

        // 当前选择的网卡名称
        private string selectedAdapterName;

        public string SelectedAdapterName
        {
            get { return selectedAdapterName; }
            set { selectedAdapterName = value; OnPropertyChanged(); }
        }


        public MainViewModel()
        {
            this.adpaterNames = new ObservableCollection<string>(NetTools.ResolveAdapter());
        }


    }



}

