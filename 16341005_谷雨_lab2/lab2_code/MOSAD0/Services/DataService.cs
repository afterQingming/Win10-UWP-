using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using MOSAD0.ViewModels;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MOSAD0.Services
{
     public static class DataService
    {
        public static void Insert( string title, string detail, DateTime date, double size,ImageSource img)
        {
            var newModel = new Models.TodoItem(title, detail, date, size, img);
            
            DatabaseService.GetInstance().Insert(newModel);
            SaveBitmapToFileAsync((WriteableBitmap)img, newModel.id);
            TodoItemViewModel.GetInstance().AddTodoItem(newModel);

            App.UpdateFileAndTile();

        }
        public static void Delete(string id)
        {
            DeleteProfilePictureAsync(id);
            DatabaseService.GetInstance().Delete(id);
            TodoItemViewModel.GetInstance().RemoveTodoItem();
            App.UpdateFileAndTile();
        }
        public static void Update(string title, string detail, DateTime date, double size, ImageSource img)
        {
            DatabaseService.GetInstance().Update(TodoItemViewModel.GetInstance().selectedItem.id, title, detail, size, date);
            SaveBitmapToFileAsync((WriteableBitmap)img, TodoItemViewModel.GetInstance().selectedItem.id);
            TodoItemViewModel.GetInstance().UpdateTodoItem(title, detail, date, size, img);
           
            App.UpdateFileAndTile();
        }
        public static string Query(string text)
        {
            return DatabaseService.GetInstance().Query(text);
        }
        public static void Complete(string id,bool IsCompleted)
        {
            DatabaseService.GetInstance().Complete(id, IsCompleted);
        }

        public static async void SaveBitmapToFileAsync(WriteableBitmap image, string userId)
        {
            StorageFolder pictureFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("ProfilePictures", CreationCollisionOption.OpenIfExists);
            var file = await pictureFolder.CreateFileAsync(userId + ".jpg", CreationCollisionOption.ReplaceExisting);

            using (var stream = await file.OpenStreamForWriteAsync())
            {
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream.AsRandomAccessStream());
                var pixelStream = image.PixelBuffer.AsStream();
                byte[] pixels = new byte[image.PixelBuffer.Length];

                await pixelStream.ReadAsync(pixels, 0, pixels.Length);

                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)image.PixelWidth, (uint)image.PixelHeight, 100, 100, pixels);

                await encoder.FlushAsync();
            }
        }

        public static async Task<WriteableBitmap> GetProfilePictureAsync(string userId)
        {

            StorageFolder pictureFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("ProfilePictures");
            StorageFile pictureFile = await pictureFolder.GetFileAsync(userId + ".jpg");

            using (IRandomAccessStream stream = await pictureFile.OpenAsync(FileAccessMode.Read))
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                WriteableBitmap bmp = new WriteableBitmap((int)decoder.PixelWidth, (int)decoder.PixelHeight);
                await bmp.SetSourceAsync(stream);
                return bmp;

            }
        }
        public static async void DeleteProfilePictureAsync(string userId)
        {

            StorageFolder pictureFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("ProfilePictures");
            StorageFile pictureFile = await pictureFolder.GetFileAsync(userId + ".jpg");
            await pictureFile.DeleteAsync();


        }

        public static void ReadFileAsync()
        {
            Services.DatabaseService.GetInstance().LoadData();
           
        }
        public static async Task<WriteableBitmap> ImgSelectorAsync()
        {
            FileOpenPicker picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.List;  //设置文件的现实方式，这里选择的是图标  
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary; //设置打开时的默认路径，这里选择的是图片库  
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            picker.FileTypeFilter.Add(".gif");                      //添加可选择的文件类型，这个必须要设置  
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();    //只能选择一个文件  
            WriteableBitmap bitmapImage = new WriteableBitmap(100, 100);
            if (file != null)
            {

                using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read))
                {
                    
                    bitmapImage.SetSource(stream);
                    DataService.SaveBitmapToFileAsync(bitmapImage, "temp");
                   
                }
            }
            return bitmapImage;
        }

        public static void SaveTemp(string title, string detail, double size, DateTime date,ImageSource img)
        {
            DatabaseService.GetInstance().SaveTemp(title, detail, size, date);
        }
        public static async void ResumeTemp(string[] temp,Image image)
        {
            DatabaseService.GetInstance().ResumeTemp(temp);
            image.Source = await GetProfilePictureAsync("temp");
        }

    }
}
