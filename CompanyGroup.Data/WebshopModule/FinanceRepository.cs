using System;
using System.Collections.Generic;

namespace CompanyGroup.Data.WebshopModule
{
    public class FinanceRepository : CompanyGroup.Domain.WebshopModule.IFinanceRepository
    {
        public FinanceRepository() { }

        /// <summary>
        /// nhibernate extract interface session
        /// </summary>
        private NHibernate.ISession ExtractInterfaceSession
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetExtractInterfaceSession(); }
        }

        private NHibernate.ISession WebInterfaceSession
        {
            get { return CompanyGroup.Data.NHibernateSessionManager.Instance.GetWebInterfaceSession(); }
        }

        /// <summary>
        /// átváltási rátát visszaadó lekérdezés
        /// </summary>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.ExchangeRate> GetCurrentRates()
        {
            NHibernate.IQuery query = ExtractInterfaceSession.GetNamedQuery("InternetUser.ExchangeRate")
                                             .SetResultTransformer(
                                             new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.ExchangeRate).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.WebshopModule.ExchangeRate>() as List<CompanyGroup.Domain.WebshopModule.ExchangeRate>;        
        }

        /// <summary>
        /// tartós bérlet legkissebb és legnagyobb értékét tartalmazó lekérdezés 
        /// </summary>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue GetMinMaxLeasingValues()
        {
            NHibernate.IQuery query = ExtractInterfaceSession.GetNamedQuery("InternetUser.MinMaxFinanceLeasingValues")
                                             .SetResultTransformer(
                                             new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue).GetConstructors()[0]));

            return query.UniqueResult<CompanyGroup.Domain.WebshopModule.MinMaxLeasingValue>();
        }

        /// <summary>
        /// kalkuláció, tartósbérlet számítás finanszírozandó összeg alapján
        /// </summary>
        /// <remarks>
        /// bejövő paraméterek validálása, 
        /// lízing információk lekérdezése a felhasználó által megadott finanszírozandó összeg alapján 
        /// számítás: 30000000 Ft input esetén GetLeasingByFinancedAmount eredménye:
        /// FinanceParameterId	LeasingIntervalId	PaymentPeriodId	InterestRate	PresentValue	NumOfMonth	PercentValue
        /// 17	                5	                1	            2.940000000000	1.056000000000	24	        4.928800000000
        /// 18	                5	                2	            2.600000000000	1.062000000000	36	        3.450400000000
        /// 19	                5	                3	            2.600000000000	1.068000000000	48	        2.724500000000
        /// 20	                5	                4	            2.460000000000	1.074000000000	60	        2.285200000000
        /// Az eredményhalmazból a FinanceParameterId és a NumOfMonth értéke változatlanul jut a kimenetre, 
        /// a CalculatedValue output mező értéke: finanszírozandó összeg * (PercentValue / 100)
        /// </remarks>
        /// <param name="amount"></param>
        /// <returns></returns>
        public List<CompanyGroup.Domain.WebshopModule.LeasingOption> GetLeasingByFinancedAmount(int amount)
        {
            NHibernate.IQuery query = ExtractInterfaceSession.GetNamedQuery("InternetUser.LeasingByFinancedAmount")
                                             .SetInt32("FinancedAmount", amount)
                                             .SetResultTransformer(
                                             new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(CompanyGroup.Domain.WebshopModule.LeasingOption).GetConstructors()[0]));

            return query.List<CompanyGroup.Domain.WebshopModule.LeasingOption>() as List<CompanyGroup.Domain.WebshopModule.LeasingOption>;
        }

        /// <summary>
        /// ajánlat kiolvasása azonosító alapján 
        /// </summary>
        /// <param name="offerId"></param>
        /// <returns></returns>
        public CompanyGroup.Domain.WebshopModule.FinanceOffer GetFinanceOffer(int offerId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((offerId > 0), "The offerId parameter must be greather than zero!");

                NHibernate.IQuery query = ExtractInterfaceSession.GetNamedQuery("InternetUser.GetFinanceOffer").SetInt32("OfferId", offerId);

                CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer = query.UniqueResult<CompanyGroup.Domain.WebshopModule.FinanceOffer>();

                return financeOffer;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár hozzáadása kollekcióhoz, új kosárazonosítóval tér vissza
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        public int Add(CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((financeOffer != null), "The financeOffer cannot be null!");

                NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.FinanceOfferInsert").SetString("VisitorId", financeOffer.VisitorId)
                                                                                                  .SetString("LeasingPersonName", financeOffer.PersonName)
                                                                                                  .SetString("LeasingAddress", financeOffer.Address)
                                                                                                  .SetString("LeasingPhone", financeOffer.Phone)
                                                                                                  .SetString("LeasingStatNumber", financeOffer.StatNumber)
                                                                                                  .SetInt32("NumOfMonth", financeOffer.NumOfMonth)
                                                                                                  .SetString("Currency", financeOffer.Currency);
                int offerId = query.UniqueResult<int>();

                return offerId;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public void Remove(int offerId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((offerId > 0), "The id parameter cannot be null!");

                NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.FianceOfferSetStatus").SetInt32("OfferId", offerId)
                                                                                                     .SetEnum("Status", CartStatus.Deleted);

                int ret = query.UniqueResult<int>();

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár feladása, WaitingForAutoPost státusz beállítás történik
        /// </summary>
        /// <param name="financeOffer"></param>
        public void Post(CompanyGroup.Domain.WebshopModule.FinanceOffer financeOffer)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((financeOffer != null), "The financeOffer cannot be null!");

                NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.ShoppingCartUpdate").SetInt32("OfferId", financeOffer.Id)
                                                                                                  .SetString("LeasingPersonName", financeOffer.PersonName)
                                                                                                  .SetString("LeasingAddress", financeOffer.Address)
                                                                                                  .SetString("LeasingPhone", financeOffer.Phone)
                                                                                                  .SetString("LeasingStatNumber", financeOffer.StatNumber)
                                                                                                  .SetInt32("NumOfMonth", financeOffer.NumOfMonth)
                                                                                                  .SetString("Currency", financeOffer.Currency);
                int ret = query.UniqueResult<int>();

                return;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár elem mennyiség frissítése
        /// </summary>
        /// <param name="lineId"></param>
        /// <param name="quantity"></param>
        public void UpdateLineQuantity(int lineId, int quantity)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((lineId > 0), "The lineId parameter cannot be null!");

                CompanyGroup.Domain.Utils.Check.Require((quantity > 0), "The quantity parameter cannot be null!");

                NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.FinanceOfferLineUpdate").SetInt32("LineId", lineId)
                                                                                                      .SetInt32("Quantity", quantity);

                int ret = query.UniqueResult<int>();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// kosár elem hozzáadás
        /// </summary>
        /// <param name="item"></param>
        public int AddLine(CompanyGroup.Domain.WebshopModule.ShoppingCartItem item)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((item != null), "The item cannot be null!");

                NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.FinanceOfferLineInsert").SetInt32("OfferId", item.CartId)
                                                                                                      .SetString("ProductId", item.ProductId)
                                                                                                      .SetInt32("Quantity", item.Quantity)
                                                                                                      .SetInt32("Price", item.CustomerPrice)
                                                                                                      .SetString("DataAreaId", item.DataAreaId)
                                                                                                      .SetEnum("Status", item.Status);
                int lineId = query.UniqueResult<int>();

                return lineId;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        ///  _posts.Collection.Update(Query.EQ("_id", postId), Update.Pull("Comments", Query.EQ("_id", commentId)).Inc("TotalComments", -1));
        /// </summary>
        /// <param name="lineId"></param>
        public void RemoveLine(int lineId)
        {
            try
            {
                CompanyGroup.Domain.Utils.Check.Require((lineId > 0), "The lineId parameter cannot be null!");

                NHibernate.IQuery query = WebInterfaceSession.GetNamedQuery("InternetUser.FinanceOfferSetLineStatus").SetInt32("LineId", lineId)
                                                                                                         .SetEnum("Status", CartItemStatus.Deleted);

                int ret = query.UniqueResult<int>();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// a megadott file elérési úttal elkészíti a pdf dokumentumot
        /// </summary>
        /// <param name="financedAmount"></param>
        /// <param name="calcValues"></param>
        /// <param name="pdfFileWithPath"></param>
        /// <returns></returns>
        public void CreateLeasingDocument(string financedAmount, System.Collections.Specialized.StringCollection calcValues, string pdfFileWithPath)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 36, 36, 36, 36); //marginTop : 72

            try
            {
                // writer letrehozas    
                iTextSharp.text.pdf.PdfWriter.GetInstance(document, new System.IO.FileStream(pdfFileWithPath, System.IO.FileMode.Create));

                // lablec
                iTextSharp.text.HeaderFooter footer = new iTextSharp.text.HeaderFooter(new iTextSharp.text.Phrase(), true);
                footer.Border = iTextSharp.text.Rectangle.NO_BORDER;
                footer.Alignment = iTextSharp.text.HeaderFooter.ALIGN_CENTER;
                document.Footer = footer;

                // dokumentum megnyitas
                document.Open();

                iTextSharp.text.Chapter chapter1 = new iTextSharp.text.Chapter(2);
                chapter1.NumberDepth = 0;

                //fejlec kep
                iTextSharp.text.Image imgHeader = GetHeaderImageFile();
                imgHeader.Alignment = iTextSharp.text.Image.ALIGN_LEFT;
                imgHeader.Alt = "NE VEDD MEG, BÉRELD!";

                iTextSharp.text.Table hTable = new iTextSharp.text.Table(1, 1);
                iTextSharp.text.Cell hCell = new iTextSharp.text.Cell(imgHeader);
                hTable.AutoFillEmptyCells = true;
                hTable.TableFitsPage = true;
                hTable.WidthPercentage = 100;
                hTable.AddCell(hCell);
                hTable.Alignment = iTextSharp.text.Table.ALIGN_LEFT;

                chapter1.Add(hTable);

                iTextSharp.text.Color defaultTextColor = new iTextSharp.text.Color(0, 0, 128);

                //uj sor, tavtarto a tabla es a fejleckep kozott
                chapter1.Add(new iTextSharp.text.Paragraph(" "));

                iTextSharp.text.pdf.BaseFont default_ttf = iTextSharp.text.pdf.BaseFont.CreateFont(CompanyGroup.Helpers.ConfigSettingsParser.GetString("FontFile", "c:\\Windows\\Fonts\\calibri.ttf"), iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED);
                iTextSharp.text.Font titlefont = new iTextSharp.text.Font(default_ttf, 16, iTextSharp.text.Font.BOLDITALIC, defaultTextColor);
                iTextSharp.text.Font defaultFont = new iTextSharp.text.Font(default_ttf, 15, iTextSharp.text.Font.NORMAL, defaultTextColor);

                //cimsor
                iTextSharp.text.Paragraph pgTitle = new iTextSharp.text.Paragraph("Finanszírozási ajánlat", titlefont);
                chapter1.Add(pgTitle);

                //tablazat
                iTextSharp.text.Table table1 = new iTextSharp.text.Table(2, 1);
                table1.BorderColor = table1.DefaultCellBorderColor = defaultTextColor;
                table1.Padding = 2;
                table1.Spacing = 1;
                table1.AutoFillEmptyCells = true;
                table1.Alignment = iTextSharp.text.Table.ALIGN_LEFT;
                table1.WidthPercentage = 80.0f;
                table1.Widths = new float[] { 60, 20 };

                iTextSharp.text.Paragraph tmpParagraph = new iTextSharp.text.Paragraph("A konfiguráció nettó vételára:", defaultFont);
                iTextSharp.text.Cell cell1 = new iTextSharp.text.Cell(tmpParagraph);
                cell1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell1.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                table1.AddCell(cell1);

                tmpParagraph = new iTextSharp.text.Paragraph(financedAmount + " Ft", defaultFont);
                iTextSharp.text.Cell cell2 = new iTextSharp.text.Cell(tmpParagraph);
                cell2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell2.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                table1.AddCell(cell2);

                chapter1.Add(table1);

                //5 oszlopos tabla 
                iTextSharp.text.Table table2 = new iTextSharp.text.Table(5);
                table2.BorderColor = defaultTextColor;
                table2.Padding = 2;
                table2.Spacing = 1;
                table2.AutoFillEmptyCells = true;
                table2.Alignment = iTextSharp.text.Table.ALIGN_LEFT;
                table2.WidthPercentage = 100.0f;
                table2.Widths = new float[] { 20, 20, 20, 20, 20 };

                //első sor
                tmpParagraph = new iTextSharp.text.Paragraph("Önerő", defaultFont);
                cell1 = new iTextSharp.text.Cell(tmpParagraph);
                cell1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell1.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell1.Header = true;
                cell1.Colspan = 4;
                cell1.BorderColor = defaultTextColor;
                table2.AddCell(cell1);

                tmpParagraph = new iTextSharp.text.Paragraph("0 Ft", defaultFont);
                cell2 = new iTextSharp.text.Cell(tmpParagraph);
                cell2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell2.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell2.BorderColor = defaultTextColor;
                table2.AddCell(cell2);

                //második sor
                //table2.AddCell("");
                tmpParagraph = new iTextSharp.text.Paragraph("Deviza: HUF", defaultFont);
                iTextSharp.text.Cell tmpCell = new iTextSharp.text.Cell(tmpParagraph);
                tmpCell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                tmpCell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                tmpCell.BorderColor = defaultTextColor;
                tmpCell.Rowspan = 2;
                table2.AddCell(tmpCell);

                tmpParagraph = new iTextSharp.text.Paragraph("Futamidő hónapokban", defaultFont);
                cell1 = new iTextSharp.text.Cell(tmpParagraph);
                cell1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell1.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell1.BorderColor = defaultTextColor;
                cell1.Colspan = 4;
                table2.AddCell(cell1);

                //harmadik sor
                tmpParagraph = new iTextSharp.text.Paragraph("24", defaultFont);
                cell2 = new iTextSharp.text.Cell(tmpParagraph);
                cell2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell2.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell2.BorderColor = defaultTextColor;
                table2.AddCell(cell2);

                tmpParagraph = new iTextSharp.text.Paragraph("36", defaultFont);
                iTextSharp.text.Cell cell3 = new iTextSharp.text.Cell(tmpParagraph);
                cell3.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell3.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell3.BorderColor = defaultTextColor;
                table2.AddCell(cell3);

                tmpParagraph = new iTextSharp.text.Paragraph("48", defaultFont);
                iTextSharp.text.Cell cell4 = new iTextSharp.text.Cell(tmpParagraph);
                cell4.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell4.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell4.BorderColor = defaultTextColor;
                table2.AddCell(cell4);

                tmpParagraph = new iTextSharp.text.Paragraph("60", defaultFont);
                iTextSharp.text.Cell cell5 = new iTextSharp.text.Cell(tmpParagraph);
                cell5.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell5.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell5.BorderColor = defaultTextColor;
                table2.AddCell(cell5);

                //negyedik sor
                tmpParagraph = new iTextSharp.text.Paragraph("Tartós bérlet", defaultFont);
                cell1 = new iTextSharp.text.Cell(tmpParagraph);
                cell1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell1.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell1.BorderColor = defaultTextColor;
                table2.AddCell(cell1);

                tmpParagraph = new iTextSharp.text.Paragraph(GetItemByPositionFromStringCollection(0, calcValues), defaultFont);
                cell2 = new iTextSharp.text.Cell(tmpParagraph);
                cell2.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell2.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell2.BorderColor = defaultTextColor;
                table2.AddCell(cell2);

                tmpParagraph = new iTextSharp.text.Paragraph(GetItemByPositionFromStringCollection(1, calcValues), defaultFont);
                cell3 = new iTextSharp.text.Cell(tmpParagraph);
                cell3.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell3.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell3.BorderColor = defaultTextColor;
                table2.AddCell(cell3);

                tmpParagraph = new iTextSharp.text.Paragraph(GetItemByPositionFromStringCollection(2, calcValues), defaultFont);
                cell4 = new iTextSharp.text.Cell(tmpParagraph);
                cell4.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell4.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell4.BorderColor = defaultTextColor;
                table2.AddCell(cell4);

                tmpParagraph = new iTextSharp.text.Paragraph(GetItemByPositionFromStringCollection(3, calcValues), defaultFont);
                cell5 = new iTextSharp.text.Cell(tmpParagraph);
                cell5.HorizontalAlignment = iTextSharp.text.Element.ALIGN_RIGHT;
                cell5.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell5.BorderColor = defaultTextColor;
                table2.AddCell(cell5);

                //ötödik sor
                table2.AddCell("");
                tmpParagraph = new iTextSharp.text.Paragraph("Nettó havidíjak", defaultFont);
                cell1 = new iTextSharp.text.Cell(tmpParagraph);
                cell1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                cell1.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                cell1.BorderColor = defaultTextColor;
                cell1.Colspan = 4;
                table2.AddCell(cell1);

                //hatodik sor
                //tmpParagraph = new iTextSharp.text.Paragraph( "A kalkulált díjak biztosítási díjat is tartalmaznak.", defaultFont );
                //cell1 = new iTextSharp.text.Cell( tmpParagraph );
                //cell1.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER;
                //cell1.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE;
                //cell1.BorderColor = defaultTextColor;
                //cell1.Colspan = 5;
                //table2.AddCell(cell1);

                chapter1.Add(table2);

                chapter1.Add(new iTextSharp.text.Paragraph(" "));

                //szoveg
                iTextSharp.text.Font smallFont = new iTextSharp.text.Font(default_ttf, 9, iTextSharp.text.Font.ITALIC, defaultTextColor);
                iTextSharp.text.Paragraph sText = new iTextSharp.text.Paragraph("HUF alapú finanszírozás, a havi díj az 1 havi Buborhoz kötött", smallFont);
                chapter1.Add(sText);

                //uj sor
                chapter1.Add(new iTextSharp.text.Paragraph(" "));

                //szoveg
                iTextSharp.text.Font bold_10_Font = new iTextSharp.text.Font(default_ttf, 10, iTextSharp.text.Font.BOLD, defaultTextColor);
                sText = new iTextSharp.text.Paragraph("Ajánlatunkat ajánlati kötöttség nélkül tettük meg!", bold_10_Font);
                chapter1.Add(sText);

                //szoveg
                sText = new iTextSharp.text.Paragraph("Az ügylet megkötéséhez a refinanszírozó jóváhagyása szükséges.", bold_10_Font);
                chapter1.Add(sText);

                //uj sor
                chapter1.Add(new iTextSharp.text.Paragraph(" "));

                ////szoveg
                //var bold_12_Font = new iTextSharp.text.Font( default_ttf, 12, iTextSharp.text.Font.BOLD, defaultTextColor );
                //sText = new iTextSharp.text.Paragraph( "Szerződéskötési díj:          0 Ft", bold_12_Font );
                //chapter1.Add(sText);

                ////uj sor
                //chapter1.Add(new iTextSharp.text.Paragraph(" "));

                //szoveg
                sText = new iTextSharp.text.Paragraph("A tartós bérlet alapvető jellemzői", bold_10_Font);
                chapter1.Add(sText);

                //szoveg
                iTextSharp.text.Font normal_10_Font = new iTextSharp.text.Font(default_ttf, 10, iTextSharp.text.Font.NORMAL, defaultTextColor);

                sText = new iTextSharp.text.Paragraph("A bérleti díjakat ÁFA terheli, mely visszaigényelhető", normal_10_Font);
                sText.IndentationLeft = 50;
                chapter1.Add(sText);

                //szoveg
                sText = new iTextSharp.text.Paragraph("Az eszköz a bérbeadó könyveiben kerül aktiválásra", normal_10_Font);
                sText.IndentationLeft = 50;
                chapter1.Add(sText);

                //szoveg
                sText = new iTextSharp.text.Paragraph("A havi díj költségként elszámolható, csökkentve ezáltal az adóalapot", normal_10_Font);
                sText.IndentationLeft = 50;
                chapter1.Add(sText);

                //uj sor
                chapter1.Add(new iTextSharp.text.Paragraph(" "));

                //szoveg
                sText = new iTextSharp.text.Paragraph("Ha bármilyen kérdése merülne fel a konstrukciót illetően, forduljon hozzánk bizalommal!", bold_10_Font);
                chapter1.Add(sText);

                //uj sor
                chapter1.Add(new iTextSharp.text.Paragraph(" "));

                //szoveg
                sText = new iTextSharp.text.Paragraph("Kublik Ádám", bold_10_Font);
                chapter1.Add(sText);

                //szoveg
                sText = new iTextSharp.text.Paragraph("értékesítési vezető", normal_10_Font);
                chapter1.Add(sText);

                //szoveg
                sText = new iTextSharp.text.Paragraph("HRP Finance", bold_10_Font);
                chapter1.Add(sText);

                //szoveg
                sText = new iTextSharp.text.Paragraph("Tel.: +36 1 452 46 16", normal_10_Font);
                chapter1.Add(sText);

                //szoveg
                sText = new iTextSharp.text.Paragraph("mob.: +36 70 452 46 16", normal_10_Font);
                chapter1.Add(sText);

                //szoveg
                sText = new iTextSharp.text.Paragraph("mail: berlet@hrpfinance.hu", normal_10_Font);
                chapter1.Add(sText);

                document.Add(chapter1);
            }
            catch (iTextSharp.text.DocumentException documentException)
            {
                throw documentException;
            }
            catch (System.IO.IOException ioeException)
            {
                throw ioeException;
            }
            finally
            {
                // dokumentum bezarasa  
                document.Close();
            }
        }

        private static string GetItemByPositionFromStringCollection(int i, System.Collections.Specialized.StringCollection sc)
        {
            try
            {
                string tmp = sc[i];
                return (!String.IsNullOrEmpty(tmp)) ? tmp + " Ft" : tmp;
            }
            catch { return String.Empty; }
        }

        /// <summary>
        /// pdf ajánlat fejléchez visszaadja a képet
        /// </summary>
        /// <returns></returns>
        private static iTextSharp.text.Image GetHeaderImageFile()
        {
            try
            {
                string headerImage = CompanyGroup.Helpers.ConfigSettingsParser.GetString("PdfHeaderJpg", @"c:\projects\2010\HrpFinance\Web\docs\header.jpg");

                if (!System.IO.File.Exists(headerImage))
                {
                    return null;
                }

                iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(headerImage);

                return img;
            }
            catch
            {
                return null;
            }
        }
    }
}
