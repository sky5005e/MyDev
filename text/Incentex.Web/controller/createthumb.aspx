<%@ Page Language="C#" %>

<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="System.Drawing" %>
<%@ Import Namespace="System.Drawing.Drawing2D" %>
<%@ Import Namespace="System.Drawing.Imaging" %>

<script runat="server">
    System.Drawing.Bitmap original_image = null;
    System.Drawing.Bitmap final_image = null;
    System.Drawing.Graphics graphic = null;
    System.Drawing.Bitmap bmPhoto = null;
    System.Drawing.Graphics grPhoto = null;
    private System.Drawing.Bitmap getResizedImage(string psFilePath, int piTargetWidth, int piTargetHeight)
    {
        if (File.Exists(psFilePath))
        {
            original_image = new System.Drawing.Bitmap(psFilePath);

            // Calculate the new width and height
            int width = original_image.Width;
            int height = original_image.Height;
            int target_width = Convert.ToInt32(piTargetWidth);
            int target_height = Convert.ToInt32(piTargetHeight);
            int new_width, new_height;

            if (height <= target_height && width <= target_width)
            {
                new_height = height;
                new_width = width;
            }
            else
            {
                float target_ratio = (float)target_width / (float)target_height;
                float image_ratio = (float)width / (float)height;

                if (target_ratio > image_ratio)
                {
                    new_height = target_height;
                    new_width = (int)Math.Floor(image_ratio * (float)target_height);
                }
                else
                {
                    new_height = (int)Math.Floor((float)target_width / image_ratio);
                    new_width = target_width;
                }

                new_width = new_width > target_width ? target_width : new_width;
                new_height = new_height > target_height ? target_height : new_height;
            }

            // Create the thumbnail

            // Old way
            //thumbnail_image = original_image.GetThumbnailImage(new_width, new_height, null, System.IntPtr.Zero);
            // We don't have to create a Thumbnail since the DrawImage method will resize, but the GetThumbnailImage looks better
            // I've read about a problem with GetThumbnailImage. If a jpeg has an embedded thumbnail it will use and resize it which
            //  can result in a tiny 40x40 thumbnail being stretch up to our target size

            final_image = new System.Drawing.Bitmap(new_width, new_height);
            graphic = System.Drawing.Graphics.FromImage(final_image);
            graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Transparent), new System.Drawing.Rectangle(0, 0, target_width, target_height));
            int paste_x = (target_width - new_width) / 2;
            int paste_y = (target_height - new_height) / 2;
            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */
            graphic.DrawImage(original_image, 0, 0, new_width, new_height);
            if (graphic != null) graphic.Dispose();
            if (original_image != null) original_image.Dispose();
            return final_image;
        }
        else
            return null;
    }

    //Created by Ankit on 6th Jan 2010
    //Below Function is added to get resized image and apply padding left and right to it..
    private System.Drawing.Image FixedSize(string imgPhoto, int Width, int Height)
    {
        if (File.Exists(imgPhoto))
        {
            original_image = new System.Drawing.Bitmap(imgPhoto);

            int sourceWidth = original_image.Width;
            int sourceHeight = original_image.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(original_image.HorizontalResolution,
                             original_image.VerticalResolution);

            grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Black);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(original_image,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            if (original_image != null) original_image.Dispose();
            return bmPhoto;
        }
        else
            return null;
    }
    //End 
</script>

<%

    MemoryStream mStream = new MemoryStream();
    System.Drawing.Image imgBGImage = null; 
    
    if (Request.QueryString["_path"] != "")
    {
        string sFilePath = "";

        if (Request.QueryString["_ty"] == "art")
        {
            sFilePath = ConfigurationSettings.AppSettings["FILESTORAGE"] + Request.QueryString["_path"];
            imgBGImage = getResizedImage(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
        }
        else if (Request.QueryString["_ty"] == "user")
        {
            sFilePath = Server.MapPath("../UploadedImages/employeePhoto/") + Request.QueryString["_path"];
           imgBGImage = FixedSize(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
          //  imgBGImage = getResizedImage(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
        }
        else if (Request.QueryString["_ty"] == "userlarge")
        {
            sFilePath = Server.MapPath("../UploadedImages/employeePhoto/") + Request.QueryString["_path"];
            imgBGImage = getResizedImage(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
        }
        else if (Request.QueryString["_ty"] == "station")
        {
            sFilePath = Server.MapPath("../UploadedImages/stationuserPhoto/") + Request.QueryString["_path"];
            imgBGImage = getResizedImage(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
        }
        else if (Request.QueryString["_ty"] == "SupplierEmployee")
        {
            sFilePath = Server.MapPath("../UploadedImages/SupplierEmployeePhoto/") + Request.QueryString["_path"];
            imgBGImage = getResizedImage(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
        }
        else if (Request.QueryString["_ty"] == "CompanyStoreDocument")
        {
            sFilePath = Server.MapPath("../UploadedImages/CompanyStoreDocuments/") + Request.QueryString["_path"];
            imgBGImage = getResizedImage(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
        }
        else if (Request.QueryString["_ty"] == "CompanyStoreContact")
        {
            sFilePath = Server.MapPath("../UploadedImages/CompanyStoreDocuments/") + Request.QueryString["_path"];
            imgBGImage = FixedSize(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
            //imgBGImage = getResizedImage(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
        }
        else if (Request.QueryString["_ty"] == "ProductImages")
        {
            sFilePath = Server.MapPath("../UploadedImages/ProductImages/Thumbs/") + Request.QueryString["_path"];
            imgBGImage = FixedSize(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
        }
        else if (Request.QueryString["_ty"] == "artImages")
        {
            sFilePath = Server.MapPath("../UploadedImages/Artwork/Thumbs/") + Request.QueryString["_path"];
            imgBGImage = FixedSize(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
        }
        else if (Request.QueryString["_ty"] == "artImagesLarge")
        {
            sFilePath = Server.MapPath("../UploadedImages/Artwork/Large/") + Request.QueryString["_path"];
            imgBGImage = FixedSize(sFilePath, Convert.ToInt32(Request.QueryString["_twidth"]), Convert.ToInt32(Request.QueryString["_theight"]));
        }
        //resizing the backgroung image
        if (Request.QueryString["_ty"] == "art")
        {
            System.Drawing.Bitmap final_WM = null;
            int new_WMwidth, new_WMheight;
            new_WMwidth = (imgBGImage.Width * 80)/100;
            new_WMheight = (imgBGImage.Height * 12)/100;

            //resizing the water mark logo according to the backgroung image
            final_WM = getResizedImage(Server.MapPath("../images/") + "watermark_logo.png", new_WMwidth, new_WMheight);
            //applying transparency to the water mark logo
            Bitmap transparentGhost = new System.Drawing.Bitmap(final_WM.Width, final_WM.Height);
            Graphics transGraphics = Graphics.FromImage(transparentGhost);
            ColorMatrix tranMatrix = new ColorMatrix();
            tranMatrix.Matrix33 = 0.7F; //this is the transparency value, tweak this to fine tuned results.

            ImageAttributes transparentAtt = new ImageAttributes();
            transparentAtt.SetColorMatrix(tranMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            transGraphics.DrawImage(final_WM, new Rectangle(0, 0, transparentGhost.Width, transparentGhost.Height), 0, 0, transparentGhost.Width, transparentGhost.Height, GraphicsUnit.Pixel, transparentAtt);
            if (transGraphics != null) transGraphics.Dispose();
            //drawing water mark logo on the backgroung image
            if (graphic != null) graphic.Dispose();
            graphic = System.Drawing.Graphics.FromImage(imgBGImage);
            graphic.DrawImage(transparentGhost, (imgBGImage.Width / 2) - (final_WM.Width / 2), (imgBGImage.Height / 2) - (final_WM.Height / 2));
            if (transparentGhost != null) transparentGhost.Dispose();
            ///*drawing water mark text*/
            //int startsize = (final_image.Width / sWaterMark.Length);//get the font size with respect to length of the string

            ////x and y cordinates to draw a string
            //x = final_image.Width / 2;
            //y = final_image.Height / 2;

            ////System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat(StringFormatFlags.DirectionVertical); -> draws a vertical string for watermark
            //System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
            //drawFormat.Alignment = StringAlignment.Center;

            //drawFormat.FormatFlags = StringFormatFlags.NoWrap;

            ////System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat(StringFormatFlags.NoWrap);

            ////drawing string on Image

            //graphic.DrawString(sWaterMark, new Font("Arial", startsize, FontStyle.Regular), new SolidBrush(Color.FromArgb(30, 255, 255, 255)), x, y, drawFormat);
            ///*drawing water mark text*/
        }

        //drawing the water mark logo on the backgroung image
        //if (File.Exists(sFilePath))
        //{
            imgBGImage.Save(mStream, Common.GetImageFormat(sFilePath));

            Response.Clear();

            string Extension = Path.GetExtension(sFilePath);
            Response.ContentType = "image/" + Extension.Substring(1, Extension.Length - 1);
            Response.BinaryWrite(mStream.GetBuffer());
            if (final_image != null) final_image.Dispose();
            if (imgBGImage != null) imgBGImage.Dispose();
            if (graphic != null) graphic.Dispose();
            if (mStream != null) mStream.Dispose();
            
            if (grPhoto != null) grPhoto.Dispose();
            if(bmPhoto != null) bmPhoto.Dispose();
            Response.End();
        //}
    }
%>
