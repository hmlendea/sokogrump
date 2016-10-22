using System;
using System.IO;

namespace SokoGrump.Utils
{
    public static class Logger
    {
        static Log logMain;

        /// <summary>
        /// Gets the main log.
        /// </summary>
        /// <value>The main log.</value>
        public static Log MainLog
        {
            get
            {
                if (logMain == null)
                    logMain = new Log("logfile.log");
                return logMain;
            }
        }
    }

    public class Log
    {
        int verbLevel = 1;
        bool enabled = true;
        bool firstUse = true;
        bool newline = true;
        string fileLocation = Environment.CurrentDirectory;
        string fileName;
        string timestampFormat = "<HH:mm:ss>";

        #region Properties

        /// <summary>
        /// Gets or sets the verbosity level.
        /// </summary>
        /// <value>The verbosity level.</value>
        public int VerbosityLevel
        {
            get { return verbLevel; }
            set
            {
                verbLevel = value;
                enabled = value != 0;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Log"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// Gets or sets the file location.
        /// </summary>
        /// <value>The file location.</value>
        public string FileLocation
        {
            get { return fileLocation; }
            set { fileLocation = value; }
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// Gets the file path.
        /// </summary>
        /// <value>The file path.</value>
        public string FilePath
        {
            get { return Path.Combine(fileLocation, fileName); }
        }

        /// <summary>
        /// Gets or sets the timestamp format.
        /// </summary>
        /// <value>The timestamp format.</value>
        public string TimestampFormat
        {
            get { return timestampFormat; }
            set { timestampFormat = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Log"/> class.
        /// </summary>
        /// <param name="fileName">File name.</param>
        public Log(string fileName)
        {
            this.fileName = fileName;
        }

        /// <summary>
        /// Write the specified text.
        /// </summary>
        /// <param name="text">Text.</param>
        public void Write(string text)
        {
            if (firstUse)
            {
                File.WriteAllText(FilePath, "");
                firstUse = false;
            }

            if (newline)
            {
                text = DateTime.Now.ToString(TimestampFormat) + " " + text;
                newline = false;
            }

            using (StreamWriter sw = File.AppendText(FilePath))
                sw.Write(text);

            Console.Write(text);
        }

        /// <summary>
        /// Write the specified text based on the verbosity level.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="verbosityLevel">Verbosity level.</param>
        public void Write(string text, int verbosityLevel)
        {
            if (verbosityLevel <= verbLevel)
                Write(text);
        }

        /// <summary>
        /// Writes the line.
        /// </summary>
        /// <param name="text">Text.</param>
        public void WriteLine(string text)
        {
            Write(text + Environment.NewLine);
            newline = true;
        }

        /// <summary>
        /// Writes the line depending on the verbosity level.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="verbosityLevel">Verbosity level.</param>
        public void WriteLine(string text, int verbosityLevel)
        {
            if (verbosityLevel <= verbLevel)
                WriteLine(text);
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="text">Text.</param>
        public void WriteError(string text)
        {
            WriteLine("ERROR: " + text + "!");
            Console.Beep();
        }

        /// <summary>
        /// Writes the error depending on the verbosity level.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="verbosityLevel">Verbosity level.</param>
        public void WriteError(string text, int verbosityLevel)
        {
            if (verbosityLevel <= verbLevel)
                WriteError(text);
        }

        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="text">Text.</param>
        public void WriteWarning(string text)
        {
            WriteLine("WARNING: " + text + "!");
        }

        /// <summary>
        /// Writes the warning depending on the verbosity level.
        /// </summary>
        /// <param name="text">Text.</param>
        /// <param name="verbosityLevel">Verbosity level.</param>
        public void WriteWarning(string text, int verbosityLevel)
        {
            if (verbosityLevel <= verbLevel)
                WriteWarning(text);
        }
    }
}
