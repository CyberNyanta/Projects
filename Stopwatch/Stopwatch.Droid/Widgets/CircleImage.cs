using System;

using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;
using Android.Views;

namespace Stopwatch.Droid.Widgets
{
    public class CircleImage : ImageView
    {

        public CircleImage(Context context) : this(context, null)
        {
        }

        public CircleImage(Context context, IAttributeSet attrs) : this(context, attrs, 0)
        {
        }

        public CircleImage(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
        {
            ClipToOutline = true;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            var border = Resources.GetDrawable(Resource.Drawable.circle, null);
            border.SetBounds(0, 0, Width, Height);
            border.Draw(canvas);
            
        }

        protected override void OnSizeChanged(int w, int h, int oldw, int oldh)
        {
            base.OnSizeChanged(w, h, oldw, oldh);
            if (w > 0 && h > 0)
                OutlineProvider = new RoundOutlineProvider(Math.Min(w, h));
        }
    }
    public class RoundOutlineProvider : ViewOutlineProvider
    {
        int size;

        public RoundOutlineProvider(int size)
        {
            if (size < 0)
                throw new InvalidOperationException("size needs to be > 0. Actually was " + size);
            this.size = size;
        }

        public override void GetOutline(View view, Outline outline)
        {
            outline.SetOval(0, 0, size, size);
        }
    }
}