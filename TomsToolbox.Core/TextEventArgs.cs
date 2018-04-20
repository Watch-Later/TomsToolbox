﻿namespace TomsToolbox.Core
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    /// Event arguments for events that deal with text, e.g. text changed or text received.
    /// </summary>
    public class TextEventArgs : EventArgs
    {
        [NotNull]
        private readonly string _text;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEventArgs"/> class.
        /// </summary>
        /// <param name="text">The text associated with the event.</param>
        public TextEventArgs([NotNull] string text)
        {
            _text = text;
        }

        /// <summary>
        /// Gets the text associated with the event.
        /// </summary>
        [NotNull]
        public string Text
        {
            get
            {
                return _text;
            }
        }
    }
}
