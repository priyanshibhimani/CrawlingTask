////using HtmlAgilityPack;
////using System;
////using System.Collections.Generic;
////using System.Data.SqlClient;
////using System.Linq;
////using System.Text;
////using System.Text.RegularExpressions;
////using System.Threading.Tasks;

////namespace AuctionDemo
////{
////    public class AuctionData
////    {
////        static string connectionString = "Data Source=DESKTOP-1TE0029;Initial Catalog=Ineichen;Integrated Security=true;"; // Add your actual connection string
////        //public void FetchData()
////        //{
////        //    // Scrape data from the website
////        //    HtmlWeb web = new HtmlWeb();
////        //    HtmlDocument doc = web.Load("https://ineichen.com/auctions/past/");
////        //    // Fetch auction item IDs from href attributes
////        //    //var ids = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");
////        //    //string pattern1 = @"auctions\/([^\/]*)\/";

////        //    //if (ids != null)
////        //    //{
////        //    //    foreach (var id in ids)
////        //    //    {
////        //    //        var i = id.GetAttributeValue("href", "");
////        //    //        Regex regex = new Regex(pattern1);
////        //    //        Match match = regex.Match(i);

////        //    //        if (match.Success)
////        //    //        {
////        //    //            var result = match.Groups[1].Value;
////        //    //            Console.WriteLine($"Auction ID: {result}");

////        //    //            // Insert into the database
////        //    //            AuctionModel auction = new AuctionModel { Id = result };
////        //    //            SaveAuctionToDatabase(auction);
////        //    //        }
////        //    //    }
////        //    //}
////        //    var ids = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");
////        //    string pattern1 = @"auctions\/([^\/]*)\/";
////        //    if (ids != null)
////        //    {
////        //        foreach (var id in ids)
////        //        {
////        //            var i = id.GetAttributeValue("href", "");
////        //            Regex regex = new Regex(pattern1);
////        //            Match match = regex.Match(i);
////        //            if (match.Success)
////        //            {
////        //                var result = match.Groups[1].Value;
////        //                Console.WriteLine(result);
////        //                AuctionModel auction = new AuctionModel { Id = result };
////        //                SaveAuctionToDatabase(auction);
////        //            }
////        //        }
////        //    }


////        //    var titleNodes = doc.DocumentNode.SelectNodes("//h2/a");

////        //    if (titleNodes != null)
////        //    {
////        //        foreach (var linkNode in titleNodes)
////        //        {
////        //            var linkText = linkNode.InnerText.Trim();
////        //            Console.WriteLine($"Title: {linkText}");

////        //            // Insert into the database
////        //            AuctionModel auction = new AuctionModel { Title = linkText };
////        //            SaveAuctionToDatabase(auction);
////        //        }
////        //    }
////        //    // Fetch auction date and location nodes
////        //    var details = doc.DocumentNode.SelectNodes("//div[@class=\"auction-date-location\"]");

////        //    if (details != null)
////        //    {
////        //        foreach (var detail in details)
////        //        {
////        //            var linkText = detail.InnerText.Trim();

////        //            // Replace multiple spaces and newlines with a single space
////        //            var cleanedText = System.Text.RegularExpressions.Regex.Replace(linkText, @"\s+", " ");

////        //            // Output to console (optional)
////        //            Console.WriteLine($"Auction Date & Location: {cleanedText}");

////        //            // Insert into the database
////        //            AuctionModel auction = new AuctionModel { Description = cleanedText };
////        //            SaveAuctionToDatabase(auction);
////        //        }
////        //    }

////        //    // Fetch image URLs from the src attribute of img elements
////        //    var imgUrls = doc.DocumentNode.SelectNodes("//a[@class=\"auction-item__image\"]/img");

////        //    if (imgUrls != null)
////        //    {
////        //        foreach (var imgUrl in imgUrls)
////        //        {
////        //            // Get the "src" attribute value, use empty string if not found
////        //            var img = imgUrl.GetAttributeValue("src", "");

////        //            // Check if img is not empty
////        //            if (!string.IsNullOrEmpty(img))
////        //            {
////        //                Console.WriteLine($"Image URL: {img}");

////        //                // Insert into the database or model
////        //                AuctionModel auction = new AuctionModel { ImageUrl = img };
////        //                SaveAuctionToDatabase(auction);
////        //            }
////        //        }
////        //    }

////        //    var links = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");

////        //    if (links != null)
////        //    {
////        //        foreach (var link in links)
////        //        {
////        //            // Get the "src" attribute value, use empty string if not found
////        //            var l = link.GetAttributeValue("href", "");

////        //            // Check if img is not empty
////        //            if (!string.IsNullOrEmpty(l))
////        //            {
////        //                Console.WriteLine(l);
////        //                AuctionModel auction = new AuctionModel { Link = l };
////        //                SaveAuctionToDatabase(auction);
////        //            }

////        //        }
////        //    }
////        //    // Fetch lotsizes from the HTML content inside the a elements
////        //    var lotsizes = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");
////        //    string pattern2 = @"\d{1,}";

////        //    if (lotsizes != null)
////        //    {
////        //        foreach (var lotsize in lotsizes)
////        //        {
////        //            // Get the inner HTML and trim any unnecessary whitespace
////        //            var lot = lotsize.InnerHtml.Trim();

////        //            // Apply the regular expression to find the lot size (digits)
////        //            Regex regex = new Regex(pattern2);
////        //            Match match = regex.Match(lot);

////        //            if (match.Success)
////        //            {
////        //                // Extract the lot size from the regex match
////        //                var result = match.Value;
////        //                Console.WriteLine($"Lot Size: {result}");

////        //                // Insert into the database or model
////        //                AuctionModel auction = new AuctionModel { LotCount = result };
////        //                SaveAuctionToDatabase(auction);
////        //            }
////        //        }
////        //    }

////        //    // Fetch date nodes using the XPath selector
////        //    var dateNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'auction-date-location__item') and not(.//b)]/text()[normalize-space() and not(preceding-sibling::br)] | //div[contains(@class, 'auction-date-location__item')]//b");

////        //    if (dateNodes != null)
////        //    {
////        //        string pattern = @"^\d{1,2}";

////        //        foreach (var linkNode in dateNodes)
////        //        {
////        //            var linkText = linkNode.InnerText.Trim();

////        //            // Apply regex to extract the date range
////        //            Match match = Regex.Match(linkText, pattern);
////        //            string dateRange = null; // Default to null

////        //            if (match.Success)
////        //            {
////        //                dateRange = match.Value;
////        //                Console.WriteLine($"Date Range: {dateRange}");
////        //            }
////        //            else
////        //            {
////        //                Console.WriteLine("NULL");
////        //            }

////        //            // Insert into the database or model, set the date as null if not found
////        //            AuctionModel auction = new AuctionModel { StartDate = dateRange };
////        //            SaveAuctionToDatabase(auction);
////        //        }
////        //    }

////        //    var titleNodes1 = doc.DocumentNode.SelectNodes("//h2/a");

////        //            if (titleNodes1 != null)
////        //            {
////        //                foreach (var linkNode in titleNodes1)
////        //                {
////        //                    var linkText = linkNode.InnerText.Trim();
////        //                    Console.WriteLine($"Title: {linkText}");

////        //                    // Insert into the database
////        //                    AuctionModel auction = new AuctionModel { StartMonth = linkText };
////        //                    SaveAuctionToDatabase(auction);
////        //                }
////        //            }
////        //            var titleNodes2 = doc.DocumentNode.SelectNodes("//h2/a");

////        //            if (titleNodes2 != null)
////        //            {
////        //                foreach (var linkNode in titleNodes2)
////        //                {
////        //                    var linkText = linkNode.InnerText.Trim();
////        //                    Console.WriteLine($"Title: {linkText}");

////        //                    // Insert into the database
////        //                    AuctionModel auction = new AuctionModel { StartYear = linkText };
////        //                    SaveAuctionToDatabase(auction);
////        //                }
////        //            }
////        //            var titleNodes3 = doc.DocumentNode.SelectNodes("//h2/a");

////        //            if (titleNodes3 != null)
////        //            {
////        //                foreach (var linkNode in titleNodes3)
////        //                {
////        //                    var linkText = linkNode.InnerText.Trim();
////        //                    Console.WriteLine($"Title: {linkText}");

////        //                    // Insert into the database
////        //                    AuctionModel auction = new AuctionModel { StartTime = linkText };
////        //                    SaveAuctionToDatabase(auction);
////        //                }
////        //            }
////        //            var titleNodes4 = doc.DocumentNode.SelectNodes("//h2/a");

////        //            if (titleNodes4 != null)
////        //            {
////        //                foreach (var linkNode in titleNodes4)
////        //                {
////        //                    var linkText = linkNode.InnerText.Trim();
////        //                    Console.WriteLine($"Title: {linkText}");

////        //                    // Insert into the database
////        //                    AuctionModel auction = new AuctionModel { EndDate = linkText };
////        //                    SaveAuctionToDatabase(auction);
////        //                }
////        //            }
////        //    var titleNodes5 = doc.DocumentNode.SelectNodes("//h2/a");

////        //    if (titleNodes5 != null)
////        //    {
////        //        foreach (var linkNode in titleNodes5)
////        //        {
////        //            var linkText = linkNode.InnerText.Trim();
////        //            Console.WriteLine($"Title: {linkText}");

////        //            // Insert into the database
////        //            AuctionModel auction = new AuctionModel { EndMonth = linkText };
////        //            SaveAuctionToDatabase(auction);
////        //        }
////        //    }
////        //    var titleNodes6 = doc.DocumentNode.SelectNodes("//h2/a");

////        //    if (titleNodes6 != null)
////        //    {
////        //        foreach (var linkNode in titleNodes6)
////        //        {
////        //            var linkText = linkNode.InnerText.Trim();
////        //            Console.WriteLine($"Title: {linkText}");

////        //            // Insert into the database
////        //            AuctionModel auction = new AuctionModel { EndYear = linkText };
////        //            SaveAuctionToDatabase(auction);
////        //        }
////        //    }
////        //    var titleNodes7 = doc.DocumentNode.SelectNodes("//h2/a");

////        //    if (titleNodes7 != null)
////        //    {
////        //        foreach (var linkNode in titleNodes7)
////        //        {
////        //            var linkText = linkNode.InnerText.Trim();
////        //            Console.WriteLine($"Title: {linkText}");

////        //            // Insert into the database
////        //            AuctionModel auction = new AuctionModel { EndTime = linkText };
////        //            SaveAuctionToDatabase(auction);
////        //        }
////        //    }
////        //    var titleNodes8 = doc.DocumentNode.SelectNodes("//h2/a");

////        //    if (titleNodes8 != null)
////        //    {
////        //        foreach (var linkNode in titleNodes8)
////        //        {
////        //            var linkText = linkNode.InnerText.Trim();
////        //            Console.WriteLine($"Title: {linkText}");

////        //            // Insert into the database
////        //            AuctionModel auction = new AuctionModel { Location = linkText };
////        //            SaveAuctionToDatabase(auction);
////        //        }
////        //    }

////        //}
////        public void FetchData()
////        {
////            // Scrape data from the website
////            HtmlWeb web = new HtmlWeb();
////            HtmlDocument doc = web.Load("https://ineichen.com/auctions/past/");

////            // Create an instance of AuctionModel to hold all the data
////            AuctionModel auction = new AuctionModel();

////            // Fetch auction item IDs from href attributes
////            var ids = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");
////            string pattern1 = @"auctions\/([^\/]*)\/";

////            if (ids != null)
////            {
////                foreach (var id in ids)
////                {
////                    var i = id.GetAttributeValue("href", "");
////                    Regex regex = new Regex(pattern1);
////                    Match match = regex.Match(i);

////                    if (match.Success)
////                    {
////                        auction.Id = match.Groups[1].Value;  // Store ID in the auction model
////                        Console.WriteLine($"Auction ID: {auction.Id}");
////                    }
////                }
////            }

////            var titleNodes = doc.DocumentNode.SelectNodes("//h2/a");
////            if (titleNodes != null)
////            {
////                foreach (var linkNode in titleNodes)
////                {
////                    auction.Title = linkNode.InnerText.Trim();  // Store title in the auction model
////                    Console.WriteLine($"Title: {auction.Title}");
////                }
////            }

////            // Fetch auction date and location nodes
////            var details = doc.DocumentNode.SelectNodes("//div[@class=\"auction-date-location\"]");
////            if (details != null)
////            {
////                foreach (var detail in details)
////                {
////                    var linkText = detail.InnerText.Trim();
////                    var cleanedText = Regex.Replace(linkText, @"\s+", " ");
////                    auction.Description = cleanedText;  // Store description in the auction model
////                    Console.WriteLine($"Auction Date & Location: {cleanedText}");
////                }
////            }

////            // Fetch image URLs from the src attribute of img elements
////            var imgUrls = doc.DocumentNode.SelectNodes("//a[@class=\"auction-item__image\"]/img");
////            if (imgUrls != null)
////            {
////                foreach (var imgUrl in imgUrls)
////                {
////                    var img = imgUrl.GetAttributeValue("src", "");
////                    if (!string.IsNullOrEmpty(img))
////                    {
////                        auction.ImageUrl = img;  // Store image URL in the auction model
////                        Console.WriteLine($"Image URL: {img}");
////                    }
////                }
////            }

////            // Fetch lot sizes from the auction item buttons
////            var lotsizes = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");
////            string pattern2 = @"\d{1,}";
////            if (lotsizes != null)
////            {
////                foreach (var lotsize in lotsizes)
////                {
////                    var lot = lotsize.InnerHtml.Trim();
////                    Regex regex = new Regex(pattern2);
////                    Match match = regex.Match(lot);
////                    if (match.Success)
////                    {
////                        auction.LotCount = match.Value;  // Store lot count in the auction model
////                        Console.WriteLine($"Lot Size: {auction.LotCount}");
////                    }
////                }
////            }

////            // Fetch dates and times
////            var dateNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'auction-date-location__item') and not(.//b)]/text()[normalize-space() and not(preceding-sibling::br)] | //div[contains(@class, 'auction-date-location__item')]//b");
////            if (dateNodes != null)
////            {
////                foreach (var linkNode in dateNodes)
////                {
////                    var linkText = linkNode.InnerText.Trim();
////                    string dateRange = Regex.Match(linkText, @"^\d{1,2}").Value;
////                    if (!string.IsNullOrEmpty(dateRange))
////                    {
////                        auction.StartDate = dateRange;  // Store start date in the auction model
////                        Console.WriteLine($"Start Date: {auction.StartDate}");
////                    }
////                }
////            }

////            // You can extend the following sections to fill in other details (month, year, time, location)
////            // Example: Start Month
////            auction.StartMonth = "January"; // Replace with actual scraping logic
////            Console.WriteLine($"Start Month: {auction.StartMonth}");

////            // Example: Start Year
////            auction.StartYear = "2024"; // Replace with actual scraping logic
////            Console.WriteLine($"Start Year: {auction.StartYear}");

////            // Example: Start Time
////            auction.StartTime = "10:00 AM"; // Replace with actual scraping logic
////            Console.WriteLine($"Start Time: {auction.StartTime}");

////            // Example: End Date
////            auction.EndDate = "12/31/2024"; // Replace with actual scraping logic
////            Console.WriteLine($"End Date: {auction.EndDate}");

////            // Example: End Month
////            auction.EndMonth = "December"; // Replace with actual scraping logic
////            Console.WriteLine($"End Month: {auction.EndMonth}");

////            // Example: End Year
////            auction.EndYear = "2024"; // Replace with actual scraping logic
////            Console.WriteLine($"End Year: {auction.EndYear}");

////            // Example: End Time
////            auction.EndTime = "11:59 PM"; // Replace with actual scraping logic
////            Console.WriteLine($"End Time: {auction.EndTime}");

////            // Example: Location
////            auction.Location = "Sample Location"; // Replace with actual scraping logic
////            Console.WriteLine($"Location: {auction.Location}");

////            // Save the auction data to the database after all properties are set
////            SaveAuctionToDatabase(auction);
////        }

////        public static void SaveAuctionToDatabase(AuctionModel auction)
////        {
////            using (SqlConnection conn = new SqlConnection(connectionString))
////            {
////                conn.Open();
////                string query = @"
////            INSERT INTO Auctions 
////            (Id, Title, Description, ImageUrl, Link, LotCount, StartDate, StartMonth, StartYear, StartTime, EndDate, EndMonth, EndYear, EndTime, Location) 
////            VALUES 
////            (@Id, @Title, @Description, @ImageUrl, @Link, @LotCount, @StartDate, @StartMonth, @StartYear, @StartTime, @EndDate, @EndMonth, @EndYear, @EndTime, @Location)";

////                using (SqlCommand cmd = new SqlCommand(query, conn))
////                {
////                    // Use parameterized query to avoid SQL injection
////                    cmd.Parameters.AddWithValue("@Id", auction.Id);
////                    cmd.Parameters.AddWithValue("@Title", auction.Title ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@Description", auction.Description ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@ImageUrl", auction.ImageUrl ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@Link", auction.Link ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@LotCount", auction.LotCount ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@StartDate", auction.StartDate ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@StartMonth", auction.StartMonth ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@StartYear", auction.StartYear ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@StartTime", auction.StartTime ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@EndDate", auction.EndDate ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@EndMonth", auction.EndMonth ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@EndYear", auction.EndYear ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@EndTime", auction.EndTime ?? (object)DBNull.Value);
////                    cmd.Parameters.AddWithValue("@Location", auction.Location ?? (object)DBNull.Value);

////                    cmd.ExecuteNonQuery();  // Execute the command
////                }
////            }
////        }

////    }
////}

////        // Function to insert auction data into the database


//using HtmlAgilityPack;
//using System;
//using System.Collections.Generic;
//using System.Data.SqlClient;
//using System.Linq;
//using System.Text.RegularExpressions;

//namespace AuctionDemo
//{
//    public class AuctionData
//    {
//        static string connectionString = "Data Source=DESKTOP-1TE0029;Initial Catalog=Ineichen;Integrated Security=true;"; // Add your actual connection string

//        public void FetchData()
//        {
//            // Scrape data from the website
//            HtmlWeb web = new HtmlWeb();
//            HtmlDocument doc = web.Load("https://ineichen.com/auctions/past/");

//            List<AuctionModel> auctions = new List<AuctionModel>(); // List to hold auction entries

//            // Fetch auction item IDs from href attributes
//            var ids = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");
//            string pattern1 = @"auctions\/([^\/]*)\/";
//            if (ids != null)
//            {
//                foreach (var id in ids)
//                {
//                    var i = id.GetAttributeValue("href", "");
//                    Regex regex = new Regex(pattern1);
//                    Match match = regex.Match(i);
//                    if (match.Success)
//                    {
//                        var result = match.Groups[1].Value;
//                        Console.WriteLine($"Auction ID: {result}");

//                        // Create auction model
//                        AuctionModel auction = new AuctionModel { Id = result };
//                        auctions.Add(auction); // Add auction to the list
//                    }
//                }
//            }

//            var titleNodes = doc.DocumentNode.SelectNodes("//h2/a");
//            if (titleNodes != null)
//            {
//                foreach (var linkNode in titleNodes)
//                {
//                    var linkText = linkNode.InnerText.Trim();
//                    Console.WriteLine($"Title: {linkText}");

//                    // Update the last auction added to the list with the title
//                    if (auctions.Count > 0)
//                        auctions.Last().Title = linkText;
//                }
//            }

//            // Fetch auction date and location nodes
//            var details = doc.DocumentNode.SelectNodes("//div[@class=\"auction-date-location\"]");
//            if (details != null)
//            {
//                foreach (var detail in details)
//                {
//                    var linkText = detail.InnerText.Trim();
//                    var cleanedText = Regex.Replace(linkText, @"\s+", " ");
//                    Console.WriteLine($"Auction Date & Location: {cleanedText}");

//                    if (auctions.Count > 0)
//                        auctions.Last().Description = cleanedText;
//                }
//            }

//            // Fetch image URLs from the src attribute of img elements
//            var imgUrls = doc.DocumentNode.SelectNodes("//a[@class=\"auction-item__image\"]/img");
//            if (imgUrls != null)
//            {
//                foreach (var imgUrl in imgUrls)
//                {
//                    var img = imgUrl.GetAttributeValue("src", "");
//                    if (!string.IsNullOrEmpty(img))
//                    {
//                        Console.WriteLine($"Image URL: {img}");
//                        if (auctions.Count > 0)
//                            auctions.Last().ImageUrl = img;
//                    }
//                }
//            }

//            var links = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");
//            if (links != null)
//            {
//                foreach (var link in links)
//                {
//                    var l = link.GetAttributeValue("href", "");
//                    if (!string.IsNullOrEmpty(l))
//                    {
//                        Console.WriteLine($"Link: {l}");
//                        if (auctions.Count > 0)
//                            auctions.Last().Link = l;
//                    }
//                }
//            }

//            // Fetch lot sizes
//            var lotSizes = doc.DocumentNode.SelectNodes("//div[@class=\"auction-item__btns\"]/a");
//            string pattern2 = @"\d{1,}";
//            if (lotSizes != null)
//            {
//                foreach (var lotSize in lotSizes)
//                {
//                    var lot = lotSize.InnerHtml.Trim();
//                    Regex regex = new Regex(pattern2);
//                    Match match = regex.Match(lot);
//                    if (match.Success)
//                    {
//                        var result = match.Value;
//                        Console.WriteLine($"Lot Size: {result}");
//                        if (auctions.Count > 0)
//                            auctions.Last().LotCount = result;
//                    }
//                }
//            }

//            // Fetch auction dates and months
//            var dateNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'auction-date-location__item') and not(.//b)]/text()[normalize-space() and not(preceding-sibling::br)] | //div[contains(@class, 'auction-date-location__item')]//b");
//            if (dateNodes != null)
//            {
//                string datePattern = @"^\d{1,}";
//                string monthPattern = @"[A-Za-z]+";
//                foreach (var linkNode in dateNodes)
//                {
//                    var linkText = linkNode.InnerText.Trim();
//                    Match dateMatch = Regex.Match(linkText, datePattern);
//                    Match monthMatch = Regex.Match(linkText, monthPattern);
//                    string dateValue = null;
//                    string monthValue = null;

//                    if (dateMatch.Success)
//                    {
//                        dateValue = dateMatch.Value;
//                        Console.WriteLine($"Date: {dateValue}");
//                    }

//                    if (monthMatch.Success)
//                    {
//                        monthValue = monthMatch.Value;
//                        Console.WriteLine($"Month: {monthValue}");
//                    }

//                    if (auctions.Count > 0)
//                    {
//                        // Update the last auction's StartDate and StartMonth
//                        if (dateValue != null)
//                            auctions.Last().StartDate = dateValue;
//                        if (monthValue != null)
//                            auctions.Last().StartMonth = monthValue;
//                    }
//                }
//            }

//            // Fetch auction end dates and locations
//            var endDateNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'auction-end-date-location')]");
//            if (endDateNodes != null)
//            {
//                string datePattern = @"^\d{1,}";
//                string monthPattern = @"[A-Za-z]+";
//                foreach (var endDateNode in endDateNodes)
//                {
                  
//                    var endDateText = endDateNode.InnerText.Trim();
//                    var endDateMatch = Regex.Match(endDateText, datePattern);
//                    var endMonthMatch = Regex.Match(endDateText, monthPattern);
//                    string endDateValue = null;
//                    string endMonthValue = null;

//                    if (endDateMatch.Success)
//                    {
//                        endDateValue = endDateMatch.Value;
//                        Console.WriteLine($"End Date: {endDateValue}");
//                    }

//                    if (endMonthMatch.Success)
//                    {
//                        endMonthValue = endMonthMatch.Value;
//                        Console.WriteLine($"End Month: {endMonthValue}");
//                    }

//                    if (auctions.Count > 0)
//                    {
//                        if (endDateValue != null)
//                            auctions.Last().EndDate = endDateValue;
//                        if (endMonthValue != null)
//                            auctions.Last().EndMonth = endMonthValue;
//                    }
//                }
//            }

//            // Example for fetching the remaining properties
//            var remainingNodes = doc.DocumentNode.SelectNodes("//div[@class=\"auction-location\"]");
//            if (remainingNodes != null)
//            {
//                foreach (var locationNode in remainingNodes)
//                {
//                    var locationText = locationNode.InnerText.Trim();
//                    Console.WriteLine($"Location: {locationText}");
//                    if (auctions.Count > 0)
//                        auctions.Last().Location = locationText;
//                }
//            }

//            // Call method to insert all auctions into the database at once
//            InsertAllAuctions(auctions);
//        }

//        public static void InsertAllAuctions(List<AuctionModel> auctions)
//        {
//            using (SqlConnection conn = new SqlConnection(connectionString))
//            {
//                conn.Open();
//                foreach (var auction in auctions)
//                {
//                    // Check if the auction ID already exists
//                    string checkQuery = "SELECT COUNT(*) FROM Auctions WHERE Id = @Id";
//                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
//                    {
//                        checkCmd.Parameters.AddWithValue("@Id", auction.Id);
//                        int exists = (int)checkCmd.ExecuteScalar();

//                        if (exists > 0)
//                        {
//                            Console.WriteLine($"Auction with ID {auction.Id} already exists. Skipping insert.");
//                            continue; // Skip to the next auction if it exists
//                        }
//                    }

//                    // Proceed with the insertion if the ID doesn't exist
//                    string query = @"
//                    INSERT INTO Auctions 
//                    (Id, Title, Description, ImageUrl, Link, LotCount, StartDate, StartMonth, StartYear, StartTime, EndDate, EndMonth, EndYear, EndTime, Location) 
//                    VALUES 
//                    (@Id, @Title, @Description, @ImageUrl, @Link, @LotCount, @StartDate, @StartMonth, @StartYear, @StartTime, @EndDate, @EndMonth, @EndYear, @EndTime, @Location)";

//                    using (SqlCommand cmd = new SqlCommand(query, conn))
//                    {
//                        // Use parameterized query to avoid SQL injection
//                        cmd.Parameters.AddWithValue("@Id", auction.Id);
//                        cmd.Parameters.AddWithValue("@Title", auction.Title ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@Description", auction.Description ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@ImageUrl", auction.ImageUrl ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@Link", auction.Link ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@LotCount", auction.LotCount ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@StartDate", auction.StartDate ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@StartMonth", auction.StartMonth ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@StartYear", auction.StartYear ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@StartTime", auction.StartTime ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@EndDate", auction.EndDate ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@EndMonth", auction.EndMonth ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@EndYear", auction.EndYear ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@EndTime", auction.EndTime ?? (object)DBNull.Value);
//                        cmd.Parameters.AddWithValue("@Location", auction.Location ?? (object)DBNull.Value);

//                        cmd.ExecuteNonQuery();  // Execute the command
//                    }
//                }
//            }
//        }
//    }
//}

