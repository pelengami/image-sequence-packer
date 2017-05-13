using System.Drawing;
using System.Threading.Tasks;

namespace ImageSequencePacker.Model
{
	internal interface IPacker
	{
		Task<Bitmap> PackAsync(PackParameters packParameters);
	}
}
