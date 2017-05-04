using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ImageClassLib;

namespace ImageClassLib
{
    public class ImageCut1
    {
        #region 剪裁图片方法
        /// <summary> 
        /// 剪裁 -- 用GDI+ 
        /// </summary> 
        /// <param name="b">原始Bitmap,即需要裁剪的图片</param> 
        /// <param name="StartX">开始坐标X</param> 
        /// <param name="StartY">开始坐标Y</param> 
        /// <param name="iWidth">宽度</param> 
        /// <param name="iHeight">高度</param> 
        /// <returns>剪裁后的Bitmap</returns> 
        public Bitmap KiCut1(Bitmap b)
        {
            if (b == null)
            {
                return null;
            }

            int w = b.Width;
            int h = b.Height;

            if (X >= w || Y >= h)
            {
                return null;
            }

            if (X + Width > w)
            {
                Width = w - X;
            }

            if (Y + Height > h)
            {
                Height = h - Y;
            }

            try
            {
                Bitmap bmpOut = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);

                Graphics g = Graphics.FromImage(bmpOut);
                // Create rectangle for displaying image.
                Rectangle destRect = new Rectangle(0, 0, Width, Height);        //所画的矩形正确，它指定所绘制图像的位置和大小。 将图像进行缩放以适合该矩形。

                // Create rectangle for source image.
                Rectangle srcRect = new Rectangle(X, Y, Width, Height);      //srcRect参数指定要绘制的 image 对象的矩形部分。 将此部分进行缩放以适合 destRect 参数所指定的矩形。

                g.DrawImage(b, destRect, srcRect, GraphicsUnit.Pixel);
                //resultG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, side, side), new System.Drawing.Rectangle(0, 0, initWidth, initHeight), System.Drawing.GraphicsUnit.Pixel);
                g.Dispose();
                return bmpOut;
            }
            catch
            {
                return null;
            }
        }
        #endregion
        #region ImageCut1类的构造函数
        public int X;
        public int Y;
        public int Width;
        public int Height;
        /// <summary>
        /// ImageCut1类的构造函数，ImageCut1类用来获取鼠标在pictureBox1控件所画矩形内的图像
        /// </summary>
        /// <param name="x表示鼠标在pictureBox1控件上按下时的横坐标"></param>
        /// <param name="y表示鼠标在pictureBox1控件上按下时的纵坐标"></param>
        /// <param name="width表示鼠标在pictureBox1控件上松开鼠标的宽度"></param>
        /// <param name="heigth表示鼠标在pictureBox1控件上松开鼠标的高度"></param>
        public ImageCut1(int x, int y, int width, int heigth)
        {
            X = x;
            Y = y;
            Width = width;
            Height = heigth;
        }
        #endregion
    }
}

namespace iSee_2015727
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();//为新建一个类，提供内存空间

            if (openfile.ShowDialog() == DialogResult.OK && (openFileDialog1.FileName != ""))//判定读取文件结果是否符合系统要求、读取文件名是否不为空集
            {
                picturebox.ImageLocation = openfile.FileName;//控件记住图片读取地址

                textbox.Text = openfile.FileName;//文本框显示文件读取路径

            }

            openfile.Dispose();//打开后，关闭对话窗
        }

        private void button2_Click(object sender, EventArgs e)//清除控件里的图片数据
        {
            this.picturebox.Image = null;//清除已加载的图片
            textbox.Text = null;
        }

        private void button3_Click(object sender, EventArgs e)//放大空间中的图片
        {
            picturebox.Width += 50;//缩小边长
            picturebox.Height += 50;// 缩小长度

        }

        private void button4_Click(object sender, EventArgs e)//缩小空间中的图片
        {
            picturebox.Width -= 50;//缩小边长
            picturebox.Height -= 50;// 缩小长度
        }

        private void button5_Click(object sender, EventArgs e)//顺时针旋转空间中的图片
        {
            Bitmap bitmap = picturebox.Image as Bitmap;

            bitmap.RotateFlip(RotateFlipType.Rotate270FlipXY);//从左开始转

            picturebox.Image = bitmap;
        }

        private void button7_Click(object sender, EventArgs e)//逆时针旋转空间中的图片
        {
            Bitmap bitmap = picturebox.Image as Bitmap;
            bitmap.RotateFlip(RotateFlipType.Rotate90FlipXY);//从右开始转
            picturebox.Image = bitmap;
        }

        private void button8_Click(object sender, EventArgs e)//保存图片
        {
            bool isSave = true;
            SaveFileDialog saveImageDialog = new SaveFileDialog();
            saveImageDialog.Title = "图片保存";//对话框显示“图片保存”
            saveImageDialog.Filter = @"jpeg|*.jpg|bmp|*.bmp|gif|*.gif";//读取图片的过滤文本类型

            if (saveImageDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveImageDialog.FileName.ToString();

                if (fileName != "" && fileName != null)
                {
                    string fileExtName = fileName.Substring(fileName.LastIndexOf(".") + 1).ToString();

                    System.Drawing.Imaging.ImageFormat imgformat = null;

                    if (fileExtName != "")//选择图片保存格式
                    {
                        switch (fileExtName)
                        {
                            case "jpg"://选择jpg时
                                imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;//指定图像类型
                                break;//停止，跳走
                            case "bmp":
                                imgformat = System.Drawing.Imaging.ImageFormat.Bmp;
                                break;
                            case "gif":
                                imgformat = System.Drawing.Imaging.ImageFormat.Gif;
                                break;
                            default:
                                MessageBox.Show("只能存取为: jpg,bmp,gif 格式");
                                isSave = false;
                                break;
                        }

                    }

                    //默认保存为JPG格式  
                    if (imgformat == null)//如果指定图像类型为空
                    {
                        imgformat = System.Drawing.Imaging.ImageFormat.Jpeg;
                    }

                    if (isSave)
                    {
                        try
                        {
                            this.picturebox.Image.Save(fileName, imgformat);
                            //MessageBox.Show("图片已经成功保存!");  
                        }
                        catch
                        {
                            MessageBox.Show("保存失败,你还没有截取过图片或已经清空图片!");
                        }
                    }

                }

            }  
           
        }
         

            
        private void button6_Click(object sender, EventArgs e)//将图色变为黑白
        {
           

            int Height = this.picturebox.Image.Height;

            int Width = this.picturebox.Image.Width;

            Bitmap bitmap = new Bitmap(Width, Height);

            Bitmap MyBitmap = (Bitmap)this.picturebox.Image;

            Color pixel;

            for (int x = 0; x < Width; x++)

                for (int y = 0; y < Height; y++)

                {

                    pixel = MyBitmap.GetPixel(x, y);

                    int r, g, b, Result = 0;

                    r = pixel.R;

                    g = pixel.G;

                    b = pixel.B;
                    //实例程序以加权平均值法产生黑白图像  
                    int iType = 2;

                    switch (iType)
                    {
                        case 0://平均值法  
                            Result = ((r + g + b) / 3);

                            break;

                        case 1://最大值法  
                            Result = r > g ? r : g;

                            Result = Result > b ? Result : b;

                            break;

                        case 2://加权平均值法  

                            Result = ((int)(0.7 * r) + (int)(0.2 * g) + (int)(0.1 * b));
                            break;
                    }

                    bitmap.SetPixel(x, y, Color.FromArgb(Result, Result, Result));

                }

            this.picturebox.Image = bitmap;
        }

        private void button9_Click(object sender, EventArgs e)
        {
           
        }

        private void picturebox_Click(object sender, EventArgs e)
        {
           
        }
        //开始画图
        private Point a1, a2;//定义两个点（启点，终点）  
        private static bool drawing = false;//设置一个启动标志

        private void picturebox_MouseDown(object sender, MouseEventArgs e)
        {
            a1 = new Point(e.X, e.Y);
            a2 = new Point(e.X, e.Y);
            drawing = true;
        }

        private void picturebox_MouseUp(object sender, MouseEventArgs e)
        {
            drawing = false;
        }

        private void picturebox_MouseMove(object sender, MouseEventArgs e)
        {
            Graphics g = picturebox.CreateGraphics();
            if (e.Button == MouseButtons.Left)
            {
                if (drawing)
                {
                    //drawing = true;  
                    Point currentPoint = new Point(e.X, e.Y);
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;//消除锯齿  
                    g.DrawLine(new Pen(Color.Red, 3), a2, currentPoint);

                    a2.X = currentPoint.X;
                    a2.Y = currentPoint.Y;
                }

            }  
        }
        //清除画上去的线条
        private void button9_Click_1(object sender, EventArgs e)
        {
            Graphics g = picturebox.CreateGraphics();
            g.Clear(Color.White);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            System.Threading.Thread.Sleep(200);
            Bitmap bit = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics g = Graphics.FromImage(bit);
            g.CopyFromScreen(new Point(0, 0), new Point(0, 0), bit.Size);//保存在bit里

            bit.Save(@"F:/截图演示.jpeg");//开创一个新的文件夹
            Image img = Image.FromFile(@"F:/截图演示.jpeg");
            picturebox.Image = img;
          
            g.Dispose();//释放资源
            this.Visible = true;//可视的
        }

        private void picturebox_DoubleClick(object sender, EventArgs e)
        {
            picturebox.Width += 50;//放大边长
            picturebox.Height += 50;// 放大长度s
        }

        #region 点击打开图像
        public string strHeadImagePath; //打开图片的路径
        Bitmap Bi;  //定义位图对像

        private void button11_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "*.gif|*.jpg|*.JPEG|*.JPEG|*.bmp|*.bmp";         //设置读取图片类型
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    strHeadImagePath = openFileDialog1.FileName;
                    //this.Show(strHeadImagePath);
                    Bi = new Bitmap(strHeadImagePath);  //使用打开的图片路径创建位图对像
                    ImageCut IC = new ImageCut(40, 112, this.pictureBox1.Width, this.pictureBox1.Height);      //实例化ImageCut类，四个参数据分别表示为：x、y、width、heigth，（40、112）表示pictureBox1的Lcation的坐标，（120、144）表示pictureBox1控件的宽度和高度
                    this.pictureBox1.Image = IC.KiCut((Bitmap)(this.GetSelectImage(this.pictureBox1.Width, this.pictureBox1.Height)));     //（120、144）表示pictureBox1控件的宽度和高度
                    //this.pictureBox1.Image = (this.GetSelectImage(120, 144));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("格式不对");
                    ex.ToString();
                }
            }
            else
            {
            }

        }

        #endregion
        #region 定义显示图像方法，即将打开的图像在pictureBox1控件显示

        public void Show(string strHeadImagePath)
        {
            this.pictureBox1.Load(@strHeadImagePath);   //
        }
        #endregion
        #region 获取图像
        /// <summary>
        /// 获取指定宽度和高度的图像即使图片和pictureBox1控件一样宽和高，返回值为图片Image
        /// </summary>
        /// <param name="Width表示宽"></param>
        /// <param name="Height表示高"></param>
        /// <returns></returns>
        public Image GetSelectImage(int Width, int Height)
        {
            //Image initImage = this.pictureBox1.Image;
            Image initImage = Bi;
            //原图宽高均小于模版，不作处理，直接保存 
            if (initImage.Width <= Width && initImage.Height <= Height)
            {
                //initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                return initImage;
            }
            else
            {
                //原始图片的宽、高 
                int initWidth = initImage.Width;
                int initHeight = initImage.Height;

                //非正方型先裁剪为正方型 
                if (initWidth != initHeight)
                {
                    //截图对象 
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //宽大于高的横图 
                    if (initWidth > initHeight)
                    {
                        //对象实例化 
                        pickedImage = new System.Drawing.Bitmap(initHeight, initHeight);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量 
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位 
                        Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                        Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                        //画图 
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置宽 
                        initWidth = initHeight;
                    }
                    //高大于宽的竖图 
                    else
                    {
                        //对象实例化
                        pickedImage = new System.Drawing.Bitmap(initWidth, initWidth);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量 
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位 
                        Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                        Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                        //画图 
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置高 
                        initHeight = initWidth;
                    }

                    initImage = (System.Drawing.Image)pickedImage.Clone();
                    //                //释放截图资源 
                    pickedG.Dispose();
                    pickedImage.Dispose();
                }

                return initImage;
            }
        }
        #endregion
        #region 点击button2按钮事件

        private void button12_Click(object sender, EventArgs e)
        {
            this.label3.Text = this.pictureBox1.Width.ToString();
            this.label4.Text = this.pictureBox1.Height.ToString();
            this.label5.Text = this.pictureBox1.Image.Width.ToString();
            this.label6.Text = this.pictureBox1.Image.Height.ToString();

        }
        #endregion
        #region 缩放、裁剪图像用到的变量
        /// <summary>
        /// 
        /// </summary>
        int x1;     //鼠标按下时横坐标
        int y1;     //鼠标按下时纵坐标
        int width;  //所打开的图像的宽
        int heigth; //所打开的图像的高
        bool HeadImageBool = false;    // 此布尔变量用来判断pictureBox1控件是否有图片
        #endregion
        #region 画矩形使用到的变量
        Point p1;   //定义鼠标按下时的坐标点
        Point p2;   //定义移动鼠标时的坐标点
        Point p3;   //定义松开鼠标时的坐标点
        #endregion
        #region 鼠标按下时发生的事件


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Cross;
            this.p1 = new Point(e.X, e.Y);
            x1 = e.X;
            y1 = e.Y;
            if (this.pictureBox1.Image != null)
            {
                HeadImageBool = true;
            }
            else
            {
                HeadImageBool = false;
            }

        }
        #endregion
        #region 移动鼠标发生的事件
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.Cursor == Cursors.Cross)
            {
                this.p2 = new Point(e.X, e.Y);
                if ((p2.X - p1.X) > 0 && (p2.Y - p1.Y) > 0)     //当鼠标从左上角向开始移动时P3坐标
                {
                    this.p3 = new Point(p1.X, p1.Y);
                }
                if ((p2.X - p1.X) < 0 && (p2.Y - p1.Y) > 0)     //当鼠标从右上角向左下方向开始移动时P3坐标
                {
                    this.p3 = new Point(p2.X, p1.Y);
                }
                if ((p2.X - p1.X) > 0 && (p2.Y - p1.Y) < 0)     //当鼠标从左下角向上开始移动时P3坐标
                {
                    this.p3 = new Point(p1.X, p2.Y);
                }
                if ((p2.X - p1.X) < 0 && (p2.Y - p1.Y) < 0)     //当鼠标从右下角向左方向上开始移动时P3坐标
                {
                    this.p3 = new Point(p2.X, p2.Y);
                }
                this.pictureBox1.Invalidate();  //使控件的整个图面无效，并导致重绘控件
            }

        }
        #endregion
        #region 松开鼠标发生的事件，实例化ImageCut1类对像
        ImageCut1 IC1;//定义所画矩形的图像对像

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (HeadImageBool)
            {
                width = this.pictureBox1.Image.Width;
                heigth = this.pictureBox1.Image.Height;
                if ((e.X - x1) > 0 && (e.Y - y1) > 0)   //当鼠标从左上角向右下方向开始移动时发生
                {
                    IC1 = new ImageCut1(x1, y1, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));    //实例化ImageCut1类
                }
                if ((e.X - x1) < 0 && (e.Y - y1) > 0)   //当鼠标从右上角向左下方向开始移动时发生
                {
                    IC1 = new ImageCut1(e.X, y1, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));   //实例化ImageCut1类
                }
                if ((e.X - x1) > 0 && (e.Y - y1) < 0)   //当鼠标从左下角向右上方向开始移动时发生
                {
                    IC1 = new ImageCut1(x1, e.Y, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));   //实例化ImageCut1类
                }
                if ((e.X - x1) < 0 && (e.Y - y1) < 0)   //当鼠标从右下角向左上方向开始移动时发生
                {
                    IC1 = new ImageCut1(e.X, e.Y, Math.Abs(e.X - x1), Math.Abs(e.Y - y1));      //实例化ImageCut1类
                }
                this.pictureBox2.Width = (IC1.KiCut1((Bitmap)(this.pictureBox1.Image))).Width;
                this.pictureBox2.Height = (IC1.KiCut1((Bitmap)(this.pictureBox1.Image))).Height;
                this.pictureBox2.Image = IC1.KiCut1((Bitmap)(this.pictureBox1.Image));
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.Cursor = Cursors.Default;
            }

        }
        #endregion
        #region 获取所选矩形图像
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public Image GetSelectImage1(int Width, int Height)
        {
            Image initImage = this.pictureBox1.Image;
            //Image initImage = Bi;
            //原图宽高均小于模版，不作处理，直接保存 
            if (initImage.Width <= Width && initImage.Height <= Height)
            {
                //initImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                return initImage;
            }
            else
            {
                //原始图片的宽、高 
                int initWidth = initImage.Width;
                int initHeight = initImage.Height;

                //非正方型先裁剪为正方型 
                if (initWidth != initHeight)
                {
                    //截图对象 
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //宽大于高的横图 
                    if (initWidth > initHeight)
                    {
                        //对象实例化 
                        pickedImage = new System.Drawing.Bitmap(initHeight, initHeight);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量 
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位 
                        Rectangle fromR = new Rectangle((initWidth - initHeight) / 2, 0, initHeight, initHeight);
                        Rectangle toR = new Rectangle(0, 0, initHeight, initHeight);
                        //画图 
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置宽 
                        initWidth = initHeight;
                    }
                    //高大于宽的竖图 
                    else
                    {
                        //对象实例化
                        pickedImage = new System.Drawing.Bitmap(initWidth, initWidth);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);
                        //设置质量 
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        //定位 
                        Rectangle fromR = new Rectangle(0, (initHeight - initWidth) / 2, initWidth, initWidth);
                        Rectangle toR = new Rectangle(0, 0, initWidth, initWidth);
                        //画图 
                        pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                        //重置高 
                        initHeight = initWidth;
                    }

                    initImage = (System.Drawing.Image)pickedImage.Clone();
                    //                //释放截图资源 
                    pickedG.Dispose();
                    pickedImage.Dispose();
                }

                return initImage;
            }
        }
        #endregion
        #region 重新绘制pictureBox1控件，即移动鼠标画矩形
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            if (HeadImageBool)
            {
                Pen p = new Pen(Color.Black, 1);//画笔
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                //Bitmap bitmap = new Bitmap(strHeadImagePath);
                Bitmap bitmap = Bi;
                Rectangle rect = new Rectangle(p3, new Size(System.Math.Abs(p2.X - p1.X), System.Math.Abs(p2.Y - p1.Y)));
                e.Graphics.DrawRectangle(p, rect);
            }
            else
            {

            }

        }
        #endregion



    }
}
