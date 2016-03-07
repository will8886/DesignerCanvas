﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Undefined.DesignerCanvas
{
    /// <summary>
    /// 用于为支持图形显示的对象提供数据。
    /// 您可以考虑从此类派生以提供附加数据或行为。
    /// </summary>
    public class GraphicalObject : INotifyPropertyChanged, IGraphicalObject
    {
        private Point _Location;

        public Point Location
        {
            get { return _Location; }
            set
            {
                if (SetProperty(ref _Location, value))
                {
                    OnPropertyChanged(nameof(Left));
                    OnPropertyChanged(nameof(Top));
                }
            }
        }

        private Size _Size;

        public Size Size
        {
            get { return _Size; }
            set
            {
                if (SetProperty(ref _Size, value))
                {
                    OnPropertyChanged(nameof(Width));
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        public double Left
        {
            get { return _Location.X; }
            set
            {
                _Location.X = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public double Top
        {
            get { return _Location.Y; }
            set
            {
                _Location.Y = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Location));
            }
        }

        public double Width
        {
            get { return _Size.Width; }
            set
            {
                _Size.Width = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Size));
            }
        }

        public double Height
        {
            get { return _Size.Height; }
            set
            {
                _Size.Height = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(Size));
            }
        }

        private ImageSource _Image;

        public ImageSource Image
        {
            get { return _Image; }
            set { SetProperty(ref _Image, value); }
        }

        public Rect Bounds => new Rect(_Location, _Size);

        /// <summary>
        /// Determines whether the object is in the specified region.
        /// </summary>
        public HitTestResult HitTest(Rect testRectangle)
        {
            var b = Bounds;
            if (testRectangle.Contains(b)) return HitTestResult.Inside;
            if (b.Contains(testRectangle)) return HitTestResult.Contains;
            if (b.IntersectsWith(testRectangle)) return HitTestResult.Intersects;
            return HitTestResult.None;
        }

        #region PropertyNotifications
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value) == false)
            {
                storage = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        public GraphicalObject()
        {
            
        }

        public GraphicalObject(float left, float top, float width, float height, ImageSource image)
        {
            _Location = new Point(left, top);
            _Size = new Size(width, height);
            _Image = image;
        }
    }
}
