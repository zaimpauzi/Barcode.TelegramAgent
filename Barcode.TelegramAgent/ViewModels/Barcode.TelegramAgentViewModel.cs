using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Barcode.TelegramAgent.Models;
using Barcode.TelegrampAgent.Commands;
using ZXing;

namespace Barcode.TelegramAgent.ViewModels
{
    class BarcodeTelegramAgentViewModel : ViewModelBase
    {
        private string printName;
        private string printAddress;
        private DelegateCommand clearCommand;
        private DelegateCommand sendCommand;
        private bool canClear;
        private List<PersonObject> personList;
        private Thread thread;


        public BarcodeTelegramAgentViewModel()
        {
            var PersonList = new List<PersonObject>();
            PersonList.Add(new PersonObject() { PersonBarcode = "123456", Name = "Zaim", Address = "1730, Jalan Koperasi, Sungai Kechil, 14300 Nibong Tebal, Pulau Pinang" });
            PersonList.Add(new PersonObject() { PersonBarcode = "1234567", Name = "Asyraf", Address = "22, Lintang Pauh Indah 4, Taman Pauh Indah, 13500 Perai, Pulau Pinang" });
            personList = PersonList;
            thread = new Thread(GetPerson);
            thread.Start();
            //GetPerson();
            //MessageBox.Show("test");
        }

        public string PrintName
        {
            get { return printName; }
            set
            {
                printName = value;
                OnPropertyChanged("printName");
            }
        }

        public string PrintAddress
        {
            get { return printAddress; }
            set
            {
                printAddress = value;
                OnPropertyChanged("printAddress");
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (clearCommand == null)
                {
                    clearCommand = new DelegateCommand(isClear, CanClear);
                }
                return clearCommand;
            }
        }

        private void isClear()
        {
            canClear = false;
            PrintName = string.Empty;
            PrintAddress = string.Empty;
            //Restart thread            
            thread.Abort();
            //Thread.Sleep(2000);
            thread = new Thread(GetPerson);
            thread.Start();
        }

        private bool CanClear()
        {
            return canClear;
        }

        public ICommand SendCommand
        {
            get
            {
                if (sendCommand == null)
                {
                    sendCommand = new DelegateCommand(isSend, CanClear);
                }
                return sendCommand;
            }
        }

        private void isSend()
        {
            var SendMessageObject = new MethodsViewModel();
            SendMessageObject.sendMessage(PrintName, PrintAddress);
            MessageBox.Show("Data Sent");

        }

        public void GetPerson()
        {
            var GetSearchObject = new MethodsViewModel();
            bool isPersonExist = false;
            while (isPersonExist == false)
            {
            var Person = GetSearchObject.isSearch(personList);
            if (Person != null)
            {
                PrintName = Person.Name;
                PrintAddress = Person.Address;
                canClear = true;
                isPersonExist = true;
            }
            else
            {
                MessageBox.Show("NO RECORD, TRY AGAIN!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            }

            }
        }


    }
}
