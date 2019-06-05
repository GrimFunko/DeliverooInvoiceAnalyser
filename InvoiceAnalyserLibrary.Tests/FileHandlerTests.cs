using System;
using Xunit;

using InvoiceAnalyserLibrary;

namespace InvoiceAnalyserLibrary.Tests
{
    public class FileHandlerTests
    {
        FileHandler fh = new FileHandler(@"C:\Users\luke\Desktop\TestFolder\TestUno");

        [Fact]
        public void CorrectFileCount()
        {
            int expected = 4;

            int actual = fh.GetPDFFileCount();

            Assert.Equal(expected, actual);
        }
    }
}
