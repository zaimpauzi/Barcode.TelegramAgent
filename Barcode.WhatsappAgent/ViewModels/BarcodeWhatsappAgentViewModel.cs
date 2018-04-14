using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Barcode.WhatsappAgent.Commands;
using Barcode.WhatsappAgent.Models;
using ZXing;

namespace Barcode.WhatsappAgent.ViewModels
{
    class BarcodeWhatsappAgentViewModel : ViewModelBase
    {
        private string printName;
        private string printAddress;
        private DelegateCommand clearCommand;
        private DelegateCommand sendCommand;
        private bool canClear;
        private List<PersonObject> personList;
        private Thread thread;


        public BarcodeWhatsappAgentViewModel()
        {
            var PersonList = new List<PersonObject>();
            PersonList.Add(new PersonObject() { PersonBarcode = "123456", Name = "Zaim", Address = "1730, Jalan Koperasi, Sungai Kechil, 14300 Nibong Tebal, Pulau Pinang" });
            PersonList.Add(new PersonObject() { PersonBarcode = "1234567", Name = "Asyraf", Address = "22, Lintang Pauh Indah 4, Taman Pauh Indah, 13500 Perai, Pulau Pinang" });
            personList = PersonList;
            thread = new Thread(GetPerson);
            thread.Start();
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
                    sendCommand = new DelegateCommand(isSend);
                }
                return sendCommand;
            }
        }

        private void isSend()
        {
            

        }

        public string getBarCode()
        {
            //VideoCapture capture = new VideoCapture();
            string Result = null;

            while (Result == null)
            {

                //var image = capture.QueryFrame();
                //Bitmap barcodeBitmap = image.ToImage<Bgr, Byte>().Bitmap; //Convert the emgu Image to BitmapImage 
                //barcodeBitmap.Save("test.bmp");
                Bitmap barcodeBitmap = new Bitmap("C:\\test.bmp");

                // create a barcode reader instance
                IBarcodeReader reader = new BarcodeReader();
                //var result = reader.Decode(barcodeBitmap);
                var result = "12345678";
                if (result != null)
                {
                    //Result = result.Text.ToString();
                    Result = result;
                }

            }
            //capture.Dispose();
            return Result;
        }

        public PersonObject isSearch(List<PersonObject> _personList)
        {
            string Barcode = getBarCode();
            var personObject = new PersonObject();
            bool isExist = false;
            foreach (var person in _personList)
            {
                if (person.PersonBarcode == Barcode)
                {
                    personObject.Name = person.Name;
                    personObject.Address = person.Address;
                    isExist = true;
                }

            }

            if (isExist)
            {
                return personObject;
            }

            else
            {
                return null;
            }
        }

        public void GetPerson()
        {
            bool isPersonExist = false;
            while (isPersonExist == false)
            {
            var Person = isSearch(personList);
            if (Person != null)
            {
                PrintName = Person.Name;
                PrintAddress = Person.Address;
                canClear = true;
                isPersonExist = true;
            }
            else
            {
                MessageBox.Show("No Record");
            }

            }
        }


    }
}
