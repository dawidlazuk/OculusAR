using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Transmitter;
using Emgu.CV;
using NUnit.Framework;
using UnityEngine;
using ViewProvision;
using ViewProvision.Contract;

public class TextureConverterTests
{

    [Test]
    public void FromImage_GivenEmguImage_ShouldCreateUnityTexture()
    {
        //arrange
        var viewProvider = new ViewProvider();
        viewProvider.SetCapture(ViewProvision.Contract.CaptureSide.Left, 0);
        viewProvider.SetCapture(ViewProvision.Contract.CaptureSide.Right, 0);
        viewProvider.UpdateFrames();
        var viewData = viewProvider.GetCurrentView();
        var image = viewData.LeftImage ?? viewData.RightImage;
        var converter = new TextureConverter();

        //act
        var texture = converter.FromImage(image);

        //assert
        Assert.AreEqual(texture.width, image.Width);
        Assert.AreEqual(texture.height, image.Height);
    }

    //[Obsolete]
    //[Test]
    //public void FromBitmap_GivenBitmap_ShouldCreateUnityTexture()
    //{
    //    //arrange
    //    var viewProvider = new ViewProvider();
    //    var viewData = viewProvider.GetCurrentView();
    //    var bitmap = viewData.LeftImage ?? viewData.RightImage;
    //    var converter = new TextureConverter();

    //    //act
    //    var texture = converter.FromBitmap(bitmap);

    //    //assert
    //    Assert.AreEqual(texture.width, bitmap.Width);
    //    Assert.AreEqual(texture.height, bitmap.Height);
    //}
}
