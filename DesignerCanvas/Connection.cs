﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Undefined.DesignerCanvas
{
    public class Connection : INotifyPropertyChanged, IGraphicalObject
    {
        public Connection()
        {
            
        }

        public Connection(Connector source, Connector sink)
        {
            Source = source;
            Sink = sink;
        }

        /*
        /// <summary>
        /// Creates a <see cref="Connection"/> that onnects two different connectors.
        /// </summary>
        public static Connection Create(Connector source, Connector sink)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            //if (sink == null) throw new ArgumentNullException(nameof(sink));
            var newInst = new Connection(source, sink);
            
        }
        */

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

        private Connector _Source;

        public Connector Source
        {
            get { return _Source; }
            set
            {
                if (_Source != value)
                {
                    if (_Source != null)
                        PropertyChangedEventManager.RemoveHandler(_Source.Owner,
                            SourceObject_PropertyChanged, nameof(value.Owner.Bounds));
                    if (value != null)
                        PropertyChangedEventManager.AddHandler(value.Owner,
                            SourceObject_PropertyChanged, nameof(value.Owner.Bounds));
                    SetProperty(ref _Source, value);
                    UpdatePositions();
                }
            }
        }

        public GraphicalObject SourceObject => _Source?.Owner;

        private Connector _Sink;

        public Connector Sink
        {
            get { return _Sink; }
            set
            {
                if (_Sink != value)
                {
                    if (_Sink != null)
                        PropertyChangedEventManager.RemoveHandler(_Sink.Owner,
                            SinkObject_PropertyChanged, nameof(value.Owner.Bounds));
                    if (value != null)
                        PropertyChangedEventManager.AddHandler(value.Owner,
                            SinkObject_PropertyChanged, nameof(value.Owner.Bounds));
                    SetProperty(ref _Sink, value);
                    UpdatePositions();
                }
            }
        }

        public GraphicalObject SinkObject => _Sink?.Owner;

        private Point _SourcePosition;

        public Point SourcePosition
        {
            get { return _SourcePosition; }
            set { SetProperty(ref _SourcePosition, value); }
        }

        private Point _SinkPosition;

        public Point SinkPosition
        {
            get { return _SinkPosition; }
            set { SetProperty(ref _SinkPosition, value); }
        }


        /// <summary>
        /// Gets the bounding rectangle of the object.
        /// </summary>
        public Rect Bounds => new Rect(_SourcePosition, _SinkPosition);

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

        #region Layout Sync


        private void SourceObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.Assert(e.PropertyName == "Bounds");
            UpdatePositions();
        }

        private void SinkObject_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Debug.Assert(e.PropertyName == "Bounds");
            UpdatePositions();

        }

        private void UpdatePositions()
        {
            if (_Source != null) SourcePosition = _Source.AbsolutePosition;
            if (_Sink != null) SinkPosition = _Sink.AbsolutePosition;
        }

        #endregion
    }
}
