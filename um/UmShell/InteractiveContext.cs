using System;

using Mono.Terminal;

using Icfp2006.UM;

namespace Icfp2006.UmShell
{
	public class InteractiveContext: IOContext
	{
		private LineEditor le_ = new LineEditor("umshell");
		
		private string line_;
		private int offset_;
		private string prompt_ = "";
		
		public InteractiveContext ()
		{
		}

		#region IOContext implementation
		public uint Input ()
		{
			if (line_ == null || offset_ == line_.Length)
			{
				Console.Write ("\r");
				line_ = le_.Edit(prompt_, "") + "\n";
				offset_ = 0;
				prompt_ = "";
			}

			return line_[offset_++];
		}

		public void Output (uint ch)
		{
			if ((char)ch == '\n')
			{
				prompt_ = "";
			}
			else
			{
				prompt_ += (char)ch;
			}
			
			Console.Write ((char)ch);
		}
		#endregion
	}
}
