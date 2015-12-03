using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

using Xamarin.Forms;

namespace ExpandListView2 {
    public class App : Application {
        public App() {
            MainPage = new MyPage();
        }
    }

    class MyPage : ContentPage {

        Bing bing = new Bing();
        Data data = new Data("");
        
        //現在、展開しているセルの行番号
        int _index = -1;

        public MyPage() {

            var listView = new ListView();
            //listView.ItemsSource = data.ar;
            listView.ItemTemplate = new DataTemplate(typeof(MyCell));
            listView.HasUnevenRows = true;

            listView.ItemSelected += (s, e) => {
                OneData tmp;
                if (_index != -1) {
                    tmp = data.ar[_index];
                    tmp.Expand = false;
                    data.ar[_index] = tmp;
                }

                _index = data.ar.IndexOf(e.SelectedItem as OneData);
                if (_index != -1) {
                    tmp = data.ar[_index];
                    tmp.Expand = true;
                    data.ar[_index] = tmp;
                }

            };

            var entry = new Entry {
                Text = "Bisuness",
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            var button = new Button { Text = "OK" };
            button.Clicked += async (s, e) => {
                var resultStr = await bing.Search(entry.Text);
                data = new Data(resultStr);
                listView.ItemsSource = data.ar;

            };

            var layout = new StackLayout {
                Orientation = StackOrientation.Horizontal,
                Children = { entry, button }
            };

            Content = new StackLayout {
                Padding = new Thickness(0,Device.OnPlatform(20,0,0), 0, 0),
                Children = { layout, listView }
            };
        }
    }

    //AOTの罠: Xamarin.Forms の ListView で System.MissingMethodException: Default constructor not found for type
    //http://espresso3389.hatenablog.com/entry/2015/01/27/040427
    class PreserveAttribute : System.Attribute {
        public bool Conditional { get; set; }
    }

    //セル用のテンプレート
    class MyCell : ViewCell {

        Image _image = new Image();
        Label _title = new Label();

        [Preserve(Conditional = true)]
        public MyCell() {

            //アイコン
            _image.WidthRequest = _image.HeightRequest = 50;
            _image.VerticalOptions = LayoutOptions.Start;
            _image.SetBinding(Image.SourceProperty, "MediaUrl");

            // タイトル
            _title.FontSize = 12;
            _title.SetBinding(Label.TextProperty, "Title");

            View = new StackLayout() {
                Padding = new Thickness(5),
                Orientation = StackOrientation.Horizontal, //横に並べる
                Children = { _image, _title }
            };
        }

        View CreateExpandView() {

            _image.WidthRequest = _image.HeightRequest = 150;
            _title.FontSize = 20;

            var size = new Label { FontSize = 12 };
            size.SetBinding(Label.TextProperty, "Size");

            var type = new Label { FontSize = 12 };
            type.SetBinding(Label.TextProperty, "ContentType");

            var displayUrl = new Label { FontSize = 12 };
            displayUrl.SetBinding(Label.TextProperty, "DisplayUrl");

            var layout = new StackLayout {
                Children = { _title, size, type,displayUrl }
            };
            return new StackLayout() {
                Padding = new Thickness(5),
                Orientation = StackOrientation.Horizontal, //横に並べる
                Children = { _image, layout }
            };
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext == null) {
                return;
            }
            var data = (OneData)BindingContext;

            var expand = data.Expand;

            if (expand) {
                Height = 150;
                View = CreateExpandView();
            }
        }
    }
    /*
    //セル用のテンプレート
    class MyCell : ViewCell {

        View normalView;
        //View expandView;

        [Preserve(Conditional = true)]
        public MyCell() {

            normalView = CreateNormalView();
            //expandView = CreateExpandView();

            View = normalView;

        }

        View CreateExpandView() {
            var imageHeight = 150;
            var fontSize = 20;
            //アイコン
            var image = new Image();
            image.WidthRequest = image.HeightRequest = imageHeight;//アイコンのサイズ
            image.VerticalOptions = LayoutOptions.Start;//アイコンを行の上に詰めて表示
            try {
                image.SetBinding(Image.SourceProperty, "MediaUrl");
            } catch (Exception ex) {
                ;
            }

            // タイトル
            var title = new Label { Font = Font.SystemFontOfSize(fontSize) };
            title.SetBinding(Label.TextProperty, "Title");

            return new StackLayout() {
                Padding = new Thickness(5),
                Orientation = StackOrientation.Horizontal, //横に並べる
                Children = { image, title }
            };
        }

        View CreateNormalView() {
            var imageHeight = 50;
            var fontSize = 12;
            //アイコン
            var image = new Image();
            image.WidthRequest = image.HeightRequest = imageHeight;//アイコンのサイズ
            image.VerticalOptions = LayoutOptions.Start;//アイコンを行の上に詰めて表示
            try {
                image.SetBinding(Image.SourceProperty, "MediaUrl");
            } catch (Exception ex) {
                ;
            }

            // タイトル
            var title = new Label { Font = Font.SystemFontOfSize(fontSize) };
            title.SetBinding(Label.TextProperty, "Title");

            return new StackLayout() {
                Padding = new Thickness(5),
                Orientation = StackOrientation.Horizontal, //横に並べる
                Children = { image, title }
            };
        }

        protected override void OnBindingContextChanged() {
            base.OnBindingContextChanged();
            if (BindingContext == null) {
                return;
            }
            var data = (OneData)BindingContext;

            var expand = data.Expand;

            if (expand) {
                Height = 100;
                //View = expandView;
                View = CreateExpandView();
            }
        }
    }
*/
}
