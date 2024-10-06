using HtmlAgilityPack;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
namespace AuctionDemo
{
    public class Data
    {
        private static string connStr = "Server=DESKTOP-1TE0029;Database=Ineichen;Integrated Security=True;";
        public static string url = "https://ineichen.com/auctions/past/";
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc;
        private static string multipleSpaceRegex = @"\s+";
        public Data()
        {
            doc = web.Load(url);
            GetAllAuctionsData();
        }
        private void GetAllAuctionsData()
        {
            string baseXPath = "//div[contains(@class,'auctions-list')]//div[@id]";
            HtmlNodeCollection auctionsNodes = doc.DocumentNode.SelectNodes(baseXPath);

            if (auctionsNodes == null || auctionsNodes.Count == 0)
            {
                Console.WriteLine("No auction nodes found.");
                return; 
            }

            Console.WriteLine($"Found {auctionsNodes.Count} auction nodes.");
            foreach (HtmlNode node in auctionsNodes)
            {
                AuctionModel model = new AuctionModel();
                SetModelData(node, model);
                bool auctionExists = CheckIfAuctionExists(model.Id);

                if (auctionExists)
                {
                    UpdateAuctionIntoDatabase(model);  
                }
                else
                {
                    InsertAuctionIntoDatabase(model);  
                }
            }
        }


        #region Set Model Data
        private static void SetModelData(HtmlNode node, AuctionModel model)
        {
            SetID(node, model);
            SetTitle(node, model);
            SetImageURL(node, model);
            SetLotSize(node, model);
            SetDescription(node, model);
            SetLinks(node, model);
            SetStartDate(node, model);
            SetStartMonth(node, model);
            SetStartYear(node, model);
            SetStartTime(node, model);
            SetEndDate(node, model);
            SetEndMonth(node, model);
            SetEndYear(node, model);
            SetEndTime(node, model);
            SetLocation(node, model);
        }
        #endregion
        private static void SetID(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: node is null.");
                    return;
                }
                string titleXPath = ".//a";
                HtmlNode titleNode = node.SelectSingleNode(titleXPath);

                if (titleNode == null)
                {
                    Console.WriteLine("Error: titleNode is null. No <a> tag found in the provided node.");
                    return;
                }
                var hrefValue = titleNode.GetAttributeValue("href", "");
                string pattern1 = @"auctions\/([^\/]*)\/";
                Regex regex = new Regex(pattern1);
                Match match = regex.Match(hrefValue);
                if (match.Success)
                {
                    model.Id = match.Groups[1].Value;
                    Console.WriteLine($"Extracted ID: {model.Id}");
                }
                else
                {
                    Console.WriteLine("Error: ID not found in href.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        #region Set Title
        private static void SetTitle(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: node is null.");
                    return;
                }
                string titleXPath = ".//h2[contains(@class,'auction-item__name')]";
                HtmlNode titleNode = node.SelectSingleNode(titleXPath);
                if (titleNode == null)
                {
                    Console.WriteLine("Error: title node not found.");
                    return;
                }
                HtmlNode anchorNode = titleNode.SelectSingleNode(".//a");

                if (anchorNode == null)
                {
                    Console.WriteLine("Error: anchor node not found within title.");
                    return;
                }
                model.Title = anchorNode.InnerText.Trim();
                Console.WriteLine($"Extracted Title: {model.Title}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        #endregion

        #region Set Description
        private static void SetDescription(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: node is null.");
                    return;
                }

                string DescriptionXPath = ".//div[@class=\"auction-date-location\"]";
                HtmlNode DescriptionXPathNode = node.SelectSingleNode(DescriptionXPath);

                if (DescriptionXPathNode == null)
                {
                    Console.WriteLine("Error: Description node not found.");
                    return;
                }

                model.Description = DescriptionXPathNode.InnerText.Trim();
                model.Description = Regex.Replace(model.Description, @"\s+", " ");
                Console.WriteLine("Description:" + model.Description);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

        #endregion
        #region SetImageURL
        private static void SetImageURL(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: node is null.");
                    return;
                }

                string imageUrlXPath = ".//a[contains(@class,'auction-item__image')]/img";
                HtmlNode imageURLNode = node.SelectSingleNode(imageUrlXPath);

                if (imageURLNode == null)
                {
                    Console.WriteLine("Error: Image url node not found.");
                    return;
                }
                string imageUrlsrc = imageURLNode.GetAttributeValue("src", string.Empty);
                Uri absoluteUrl = new Uri(new Uri(url), imageUrlsrc);
                model.ImageUrl = absoluteUrl.ToString().Trim();
                Console.WriteLine(model.ImageUrl);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
        #region Links
        private static void SetLinks(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: node is null.");
                    return;
                }

                string linkXPath = ".//a";
                HtmlNode linkNode = node.SelectSingleNode(linkXPath);

                if (linkNode == null)
                {
                    Console.WriteLine("Error: Image url node not found.");
                    return;
                }
                string link = linkNode.GetAttributeValue("href", string.Empty);
                Uri absoluteUrl = new Uri(new Uri(url), link);
                model.Link = absoluteUrl.ToString().Trim();
                Console.WriteLine("Link:" + model.Link);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
        private static void SetLotSize(HtmlNode node, AuctionModel model)
        {
            try
            {

                string lotSizeXPath = ".//div[@class=\"auction-item__btns\"]/a";
                HtmlNode titleNode = node.SelectSingleNode(lotSizeXPath);

                if (titleNode == null)
                {
                    Console.WriteLine("Error: titleNode is null. No matching <a> tag found.");
                    return;
                }

                var t = titleNode.InnerText.Trim();
                string pattern1 = @"\d{1,}"; 
                Regex regex = new Regex(pattern1);
                Match match = regex.Match(t);

                if (match.Success)
                {
                    model.LotCount = match.Value;
                    Console.WriteLine($"Extracted Lot Count: {model.LotCount}");
                }
                else
                {
                    Console.WriteLine("Error: No numeric value found in the text.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #region StartDate
        private static void SetStartDate(HtmlNode node, AuctionModel model)
        {
            try
            {
                string startDateXPath = ".//div[contains(@class, 'auction-date-location__item')]/text()[normalize-space() and not(preceding-sibling::br)] | .//div[contains(@class, 'auction-date-location__item')]//b";
                HtmlNode startDateNode = node.SelectSingleNode(startDateXPath);

                if (startDateNode == null)
                {
                    Console.WriteLine("Error: startDateNode is null.");
                    return;
                }

                var t = startDateNode.InnerText.Trim();
                string pattern1 = @"^\d{1,2}";
                Regex regex = new Regex(pattern1);
                Match match = regex.Match(t);

                if (match.Success)
                {
                    model.StartDate = match.Value;
                    Console.WriteLine("Extracted Start Date: " + model.StartDate);
                }
                else
                {
                    Console.WriteLine("Error: No numeric value found in the text.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
        #region StartMonth
        private static void SetStartMonth(HtmlNode node, AuctionModel model)
        {
            try
            {
                string startMonthXPath = ".//div[contains(@class, 'auction-date-location__item')]/text()[normalize-space() and not(preceding-sibling::br)] | .//div[contains(@class, 'auction-date-location__item')]//b";
                HtmlNode startMonthNode = node.SelectSingleNode(startMonthXPath);

                if (startMonthNode == null)
                {
                    Console.WriteLine("Error: startDateNode is null.");
                    return;
                }

                var t = startMonthNode.InnerText.Trim();
                string pattern1 = @"^(\s+)?\d+\s([A-z]+)"; 
                Regex regex = new Regex(pattern1);
                Match match = regex.Match(t);

                if (match.Success)
                {
                    model.StartMonth = match.Groups[2].Value;
                    Console.WriteLine("StartMonth:" + model.StartMonth);
                }
                else
                {
                    model.StartMonth = null;
                    Console.WriteLine("StartMonth:" + model.StartMonth);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
        #region StartYear
        private static void SetStartYear(HtmlNode node, AuctionModel model)
        {
            try
            {
                string startYearXPath = ".//div[contains(@class, 'auction-date-location__item')]/text()[normalize-space() and not(preceding-sibling::br)] | .//div[contains(@class, 'auction-date-location__item')]//b";
                HtmlNode startYearNode = node.SelectSingleNode(startYearXPath);

                if (startYearNode == null)
                {
                    Console.WriteLine("Error: startDateNode is null.");
                    return;
                }

                var t = startYearNode.InnerText.Trim();

                string pattern1 = @"(\d{1,2}\s*-\s*[A-Z]+\s*(\d{4}))|\b(\d{4})\b(?=\s*(-|,))"; 
                Regex regex = new Regex(pattern1);
                Match match = regex.Match(t);

                if (match.Success)
                {
                    model.StartYear = match.Groups[3].Value;
                    Console.WriteLine("StartYear" + model.StartYear);
                }
                else
                {
                    model.StartYear = null;
                    Console.WriteLine("StartYear" + model.StartYear);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
        #region SatrtTime
        private static void SetStartTime(HtmlNode node, AuctionModel model)
        {
            try
            {
                string startTimeXPath = ".//div[@class=\"auction-date-location\"]";
                HtmlNode startTimeNode = node.SelectSingleNode(startTimeXPath);

                if (startTimeNode == null)
                {
                    Console.WriteLine("Error: startDateNode is null.");
                    return;
                }

                var t = startTimeNode.InnerText.Trim();
                string pattern = @"(\d{1,2}:\d{2} (?:CET|\(CET\)))";
                var matches = Regex.Matches(t, pattern);

                if (matches.Count > 0)
                {
                    string smallestMatch = matches[0].Value;
                    foreach (Match match in matches)
                    {
                        if (match.Value.Length < smallestMatch.Length)
                        {
                            smallestMatch = match.Value;
                        }
                    }
                    model.StartTime = smallestMatch;
                    Console.WriteLine("StartTime:" + model.StartTime);
                }
                else
                {
                    model.StartTime = null;
                    Console.WriteLine("StartTime:" + model.StartTime);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
        #region EndDate
        private static void SetEndDate(HtmlNode node, AuctionModel model)
        {
            try
            {
                string endDateXPath = ".//div[contains(@class, 'auction-date-location__item')]/text()[normalize-space() and not(preceding-sibling::br)] | .//div[contains(@class, 'auction-date-location__item')]//b";
                HtmlNode endDateNode = node.SelectSingleNode(endDateXPath);

                if (endDateNode == null)
                {
                    Console.WriteLine("Error: endDateNode is null.");
                    return;
                }

                var t = endDateNode.InnerText.Trim();
                string pattern1 = @"-\s*(\d+)"; 
                Regex regex = new Regex(pattern1);
                Match match = regex.Match(t);

                if (match.Success)
                {
                    model.EndDate = match.Groups[1].Value;
                    Console.WriteLine("Extracted endDate: " + model.EndDate);
                }
                else
                {
                    model.EndDate = null;
                    Console.WriteLine("Extracted endDate: " + model.EndDate);
                    //Console.WriteLine("Error: No numeric value found in the text.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
        #region EndMonth
        private static void SetEndMonth(HtmlNode node, AuctionModel model)
        {
            try
            {
                string endMonthXPath = ".//div[contains(@class, 'auction-date-location__item')]/text()[normalize-space() and not(preceding-sibling::br)] | .//div[contains(@class, 'auction-date-location__item')]//b";
                HtmlNode endMonthNode = node.SelectSingleNode(endMonthXPath);

                if (endMonthNode == null)
                {
                    Console.WriteLine("Error: endMonthNode is null.");
                    return;
                }

                var t = endMonthNode.InnerText.Trim();
                string pattern1 = @"-\s*\d+\s([A-Z]+)";
                Regex regex = new Regex(pattern1);
                Match match = regex.Match(t);

                if (match.Success)
                {
                    // Extract the start date and set it to model
                    model.EndMonth = match.Groups[1].Value;
                    Console.WriteLine("EndMonth:" + model.EndMonth);
                }
                else
                {
                    model.EndMonth = null;
                    Console.WriteLine("null" + model.EndMonth);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
        #region EndYear
        private static void SetEndYear(HtmlNode node, AuctionModel model)
        {
            try
            {
                string endYearXPath = ".//div[contains(@class, 'auction-date-location__item')]/text()[normalize-space() and not(preceding-sibling::br)] | .//div[contains(@class, 'auction-date-location__item')]//b";
                HtmlNode endYearNode = node.SelectSingleNode(endYearXPath);

                if (endYearNode == null)
                {
                    Console.WriteLine("Error: startDateNode is null.");
                    return;
                }

                var t = endYearNode.InnerText.Trim();

                string pattern1 = @"-\s*\d+\s[A-Z]+\s(\d+)$";
                Regex regex = new Regex(pattern1);
                Match match = regex.Match(t);

                if (match.Success)
                {
                  
                    model.EndYear = match.Groups[1].Value;
                    Console.WriteLine("EndYear" + model.EndYear);
                }
                else
                {
                    model.EndYear = null;
                    Console.WriteLine(model.EndYear);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
        #region EndTime
        private static void SetEndTime(HtmlNode node, AuctionModel model)
        {
            try
            {
                string endTimeXPath = ".//div[@class=\"auction-date-location\"]";
                HtmlNode endTimeNode = node.SelectSingleNode(endTimeXPath);
                if (endTimeNode == null)
                {
                    Console.WriteLine("Error: startDateNode is null.");
                    return;
                }

                var t = endTimeNode.InnerText.Trim();
                string pattern = @"(\d{1,2}:\d{2}\s*CET)(?=\s+[A-Za-z])";
                Regex regex = new Regex(pattern);
                Match match = regex.Match(t);

                if (match.Success)
                {
                    model.EndTime = match.Groups[1].Value;
                    Console.WriteLine("EndTime:" + model.EndTime);
                }
                else
                {
                    model.EndTime = null;
                    Console.WriteLine("null" + model.EndTime);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
        #region Set Location
        private static void SetLocation(HtmlNode node, AuctionModel model)
        {
            try
            {
                if (node == null)
                {
                    Console.WriteLine("Error: node is null.");
                    return;
                }

                string locationXPath = ".//div[@class=\"auction-date-location__item\"][2]/span";
                HtmlNode locationNode = node.SelectSingleNode(locationXPath);

                if (locationNode == null)
                {
                    Console.WriteLine("Error: Location node not found.");
                    return;
                }
                model.Location = locationNode.InnerText.Trim();
                Console.WriteLine(model.Location);



            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }
        #endregion
     

        #region Insert Auction Into Database
        public static void InsertAuctionIntoDatabase(AuctionModel model)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand command = new SqlCommand("PR_Auctions_InsertAuction", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", model.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Title", model.Title ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Description", model.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ImageUrl", model.ImageUrl ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Link", model.Link ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LotCount", model.LotCount ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartDate", model.StartDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartMonth", model.StartMonth ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartYear", model.StartYear ?? (object)DBNull.Value); 
                command.Parameters.AddWithValue("@StartTime", model.StartTime ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndDate", model.EndDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndMonth", model.EndMonth ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndYear", model.EndYear ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndTime", model.EndTime ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Location", model.Location);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("Rows affected: " + rowsAffected);
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        #endregion
        #region Update Auction Into Database
        public static void UpdateAuctionIntoDatabase(AuctionModel model)
        {
            try
            {
                SqlConnection conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand command = new SqlCommand("PR_Auctions_UpdateByAuctionID", conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Id", model.Id ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Title", model.Title ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Description", model.Description ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@ImageUrl", model.ImageUrl ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Link", model.Link ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@LotCount", model.LotCount ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartDate", model.StartDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartMonth", model.StartMonth ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@StartYear", model.StartYear ?? (object)DBNull.Value); 
                command.Parameters.AddWithValue("@StartTime", model.StartTime ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndDate", model.EndDate ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndMonth", model.EndMonth ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndYear", model.EndYear ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@EndTime", model.EndTime ?? (object)DBNull.Value);
                command.Parameters.AddWithValue("@Location", model.Location);
                int rowsAffected = command.ExecuteNonQuery();
                Console.WriteLine("Updated Rows affected: " + rowsAffected);
                conn.Close();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
        #endregion
        private bool CheckIfAuctionExists(string auctionId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("PR_Auctions_CheckIfExists", conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Id", auctionId);
                    return (int)command.ExecuteScalar() > 0;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error: " + ex.Message);
                return false;  
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;  
            }
        }

    }
}
