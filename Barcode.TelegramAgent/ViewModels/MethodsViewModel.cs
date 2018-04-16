using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Barcode.TelegramAgent.Models;
using System.Drawing;
using ZXing;
using Emgu.CV;
using Emgu.CV.Structure;

namespace Barcode.TelegramAgent.ViewModels
{
    class MethodsViewModel
    {

       private static TelegramBotClient Bot = new TelegramBotClient("557138021:AAEGRCVfFzgvZpMAxyuHoYiHqesGDM7p53c");

        public string getBarCode()
        {
            VideoCapture capture = new VideoCapture();
            string Result = null;

            while (Result == null)
            {

                var image = capture.QueryFrame();
                Bitmap barcodeBitmap = image.ToImage<Bgr, Byte>().Bitmap; //Convert the emgu Image to BitmapImage 
                //barcodeBitmap.Save("test.bmp");
                //Bitmap barcodeBitmap = new Bitmap("C:\\test.bmp");

                // create a barcode reader instance
                IBarcodeReader reader = new BarcodeReader();
                var result = reader.Decode(barcodeBitmap);
                //var result = "123456";
                if (result != null)
                {
                    Result = result.Text.ToString();
                    //Result = result;
                }

            }
            capture.Dispose();
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

        public void sendMessage(string _name, string _address)
        {
            string sendText = String.Format("*NAME:* \n_{0}_ \n*ADDRESS:* \n_{1}_", _name, _address);
         
            Bot.SendTextMessageAsync(457279920, sendText, Telegram.Bot.Types.Enums.ParseMode.Markdown);
        }


    }
}
