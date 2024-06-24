using EchoesOfSerenity.Core;
using EchoesOfSerenity.Layers;

namespace EchoesOfSerenity;

public class Echoes : Game
{
    protected override void OnInit()
    {
        ConstructLayer<TestLayer>();
    }
}
