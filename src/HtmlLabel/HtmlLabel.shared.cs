﻿using System;
using System.Text;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace LabelHtml.Forms.Plugin.Abstractions
{
	/// <inheritdoc />
	/// <summary>
	/// A label that is able to display HTML content
	/// </summary>
	public class HtmlLabel : Label
	{
		/// <summary>
		/// Set the value of the MaxLines property
		/// </summary>
		/// <param name="view"></param>
		/// <param name="value"></param>
		public static void SetMaxLines(BindableObject view, int value)
		{
			view?.SetValue(MaxLinesProperty, value);
		}

		/// <summary>
		/// Send the Navigating event
		/// </summary>
		/// <param name="args"></param>
		internal void SendNavigating(WebNavigatingEventArgs args)
		{
			Navigating?.Invoke(this, args);
		}

		/// <summary>
		/// Send the Navigated event
		/// </summary>
		/// <param name="args"></param>
		internal void SendNavigated(WebNavigatingEventArgs args)
	    {
		    Navigated?.Invoke(this, args);
	    }

		/// <summary>
		/// Fires before the open URL request is done.
		/// </summary>
		public event EventHandler<WebNavigatingEventArgs> Navigating;

		/// <summary>
		/// Fires when the open URL request is done.
		/// </summary>
		public event EventHandler<WebNavigatingEventArgs> Navigated;
	}


    /// <summary>
	/// 
	/// </summary>
    public class LabelRendererHelper
    {
		private readonly Label _label;
		private readonly string _text;
		private StringBuilder _builder;

		public LabelRendererHelper(Label label, string text)
		{
			_label = label ?? throw new ArgumentNullException(nameof(label));
			_text = text?.Trim() ?? throw new ArgumentNullException(nameof(text));
			_builder = new StringBuilder();
		}

		private void SetFontAttributes()
		{
			if (_label.FontAttributes == FontAttributes.None)
            {
                return;
            }

            switch (_label.FontAttributes)
			{
				case FontAttributes.Bold:
                    _builder = _builder.Append("font-weight: bold; ");
                    break;
				case FontAttributes.Italic:
                    _builder = _builder.Append("font-style: italic; ");
					break;
			}
		}

        private void SetFontFamily(bool includeAppleSystem = false)
        {
            if (_label.FontFamily == null)
            {
                return;
            }
            _builder = _builder.Append($"font-family: '");
            if (includeAppleSystem)
            {
                _builder = _builder.Append("-apple-system', '");
            }
            _builder = _builder.Append($"{_label.FontFamily}'; ");
        }

		private void SetFontSize()
		{
			if (Math.Abs(_label.FontSize - 14d) < 0.000000001)
            {
                return;
            }

            _builder = _builder.Append($"font-size: {_label.FontSize}px; ");
		}

		private void SetTextColor()
		{
			if (_label.TextColor.IsDefault)
            {
                return;
            }

            Color color = _label.TextColor;
			var red = (int)(color.R * 255);
			var green = (int)(color.G * 255);
			var blue = (int)(color.B * 255);
			var alpha = (int)(color.A * 255);
			var hex = $"#{red:X2}{green:X2}{blue:X2}{alpha:X2}";
			_builder = _builder.Append($"color: {hex}; ");
		}

		private void SetHorizontalTextAlign()
		{
			if (_label.HorizontalTextAlignment == TextAlignment.Start)
            {
                return;
            }

            switch (_label.HorizontalTextAlignment)
			{
				case TextAlignment.Center:
					_builder = _builder.Append("text-align: center; ");
					break;
				case TextAlignment.End:
					_builder = _builder.Append("text-align: right; ");
					break;
			}
		}

		public override string ToString()
		{
			return ToString(false);
		}

		public string ToString(bool isAppleSystem)
		{
			if (string.IsNullOrWhiteSpace(_label.Text))
            {
                return string.Empty;
            }

            _builder = _builder.Append("<div style=\"");
			SetFontAttributes();
			SetFontFamily(isAppleSystem);
			SetFontSize();
			SetTextColor();
			SetHorizontalTextAlign();
			_builder = _builder.Append($"\">{_text}</div>");
			var text = _builder.ToString();
			return text;
		}
    }
}