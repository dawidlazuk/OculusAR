using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Transmitter;
using Emgu.CV;
using NUnit.Framework;
using ViewProvision;

    public class TextureConverterTests
    {

        [Test]
        public void FromImage_GivenEmguImage_ShouldCreateUnityTexture()
        {
            //arrange
            var viewProvider = new ViewProvider();
            var viewDataInternal = viewProvider.GetCurrentViewInternal();
            var image = viewDataInternal.LeftImage ?? viewDataInternal.RightImage;
            var converter = new TextureConverter();

            //act
            var texture = converter.FromImage(image);

            //assert
            Assert.AreEqual(texture.width, image.Width);
            Assert.AreEqual(texture.height, image.Height);
        }

        [Test]
        public void FromBitmap_GivenBitmap_ShouldCreateUnityTexture()
        {
            //arrange
            var viewProvider = new ViewProvider();
            var viewData = viewProvider.GetCurrentView();
            var bitmap = viewData.LeftImage ?? viewData.RightImage;
            var converter = new TextureConverter();

            //act
            var texture = converter.FromBitmap(bitmap);

            //assert
            Assert.AreEqual(texture.width, bitmap.Width);
            Assert.AreEqual(texture.height, bitmap.Height);
        }
    }
