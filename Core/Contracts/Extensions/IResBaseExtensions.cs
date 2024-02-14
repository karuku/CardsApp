using Contracts.ResModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Extensions
{
	public static class IResBaseExtensions
	{
		public static bool IsSuccessful(this IResBase res)
		{
			return res.ResCode == 0;
		}
		
	}
}
