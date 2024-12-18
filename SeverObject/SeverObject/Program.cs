using System;
using System.Windows.Forms;
using Server;

namespace Core
{

    public class Program
    {

		[STAThread]
		static void Main(string[] args)
        {
		
            Application.EnableVisualStyles();

            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new ServerPresenter());
        }
	}
}
