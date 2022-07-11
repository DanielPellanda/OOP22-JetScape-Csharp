//Not implemented
//using Graphics2D;

namespace Jetspcape.Background
{
	public interface IBackground
	{
		void SetVisibility(bool v);

		bool IsVisible();

		void Reset();

		void Update();

		void Draw(Graphics2D g);

		void DrawCoordinates(Graphics2D g);
	}
}
