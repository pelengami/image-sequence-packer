using System.Drawing;
using System.Threading.Tasks;

namespace SpriteSheetPacker.Model
{
	internal interface IPacker
	{
		Task<Bitmap> PackAsync(PackParameters packParameters);
	}
}
