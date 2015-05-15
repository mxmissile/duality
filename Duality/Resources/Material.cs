﻿using System;
using System.Collections.Generic;
using System.Linq;

using Duality.Drawing;
using Duality.Properties;
using Duality.Editor;
using Duality.Cloning;

namespace Duality.Resources
{
	/// <summary>
	/// Materials are standardized <see cref="BatchInfo">BatchInfos</see>, stored as a Resource. 
	/// Just like BatchInfo objects, they describe how an object, represented by a set of vertices, 
	/// looks like. Using Materials is generally more performant than using BatchInfos but not always
	/// reasonable, for example when there is a single, unique GameObject with a special appearance:
	/// This is a typical <see cref="BatchInfo"/> case.
	/// </summary>
	/// <seealso cref="BatchInfo"/>
	[ExplicitResourceReference(typeof(Texture))]
	[EditorHintCategory(typeof(CoreRes), CoreResNames.CategoryGraphics)]
	[EditorHintImage(typeof(CoreRes), CoreResNames.ImageMaterial)]
	public class Material : Resource
	{
		/// <summary>
		/// A solid, white Material.
		/// </summary>
		public static ContentRef<Material> SolidWhite			{ get; private set; }
		/// <summary>
		/// A solid, black Material.
		/// </summary>
		public static ContentRef<Material> SolidBlack			{ get; private set; }
		/// <summary>
		/// A Material that inverts its background.
		/// </summary>
		public static ContentRef<Material> InvertWhite			{ get; private set; }
		/// <summary>
		/// A Material showing the Duality icon.
		/// </summary>
		public static ContentRef<Material> DualityIcon			{ get; private set; }
		/// <summary>
		/// A Material showing the Duality icon, but without the text on it.
		/// </summary>
		public static ContentRef<Material> DualityIconB			{ get; private set; }
		/// <summary>
		/// A Material showing the Duality logo.
		/// </summary>
		public static ContentRef<Material> DualityLogoBig		{ get; private set; }
		/// <summary>
		/// A Material showing the Duality logo.
		/// </summary>
		public static ContentRef<Material> DualityLogoMedium	{ get; private set; }
		/// <summary>
		/// A Material showing the Duality logo.
		/// </summary>
		public static ContentRef<Material> DualityLogoSmall		{ get; private set; }
		/// <summary>
		/// A Material showing a black and white checkerboard.
		/// </summary>
		public static ContentRef<Material> Checkerboard			{ get; private set; }

		internal static void InitDefaultContent()
		{
			const string VirtualContentPath				= ContentProvider.VirtualContentPath + "Material:";
			const string ContentPath_SolidWhite			= VirtualContentPath + "SolidWhite";
			const string ContentPath_SolidBlack			= VirtualContentPath + "SolidBlack";
			const string ContentPath_InvertWhite		= VirtualContentPath + "InvertWhite";
			const string ContentPath_DualityIcon		= VirtualContentPath + "DualityIcon";
			const string ContentPath_DualityIconB		= VirtualContentPath + "DualityIconB";
			const string ContentPath_DualityLogoBig		= VirtualContentPath + "DualityLogoBig";
			const string ContentPath_DualityLogoMedium	= VirtualContentPath + "DualityLogoMedium";
			const string ContentPath_DualityLogoSmall	= VirtualContentPath + "DualityLogoSmall";
			const string ContentPath_Checkerboard		= VirtualContentPath + "Checkerboard";

			ContentProvider.AddContent(ContentPath_SolidWhite, new Material(DrawTechnique.Solid, ColorRgba.White));
			ContentProvider.AddContent(ContentPath_SolidBlack, new Material(DrawTechnique.Solid, ColorRgba.Black));
			ContentProvider.AddContent(ContentPath_InvertWhite, new Material(DrawTechnique.Invert, ColorRgba.White));
			ContentProvider.AddContent(ContentPath_DualityIcon, new Material(DrawTechnique.Mask, ColorRgba.White, Texture.DualityIcon));
			ContentProvider.AddContent(ContentPath_DualityIconB, new Material(DrawTechnique.Mask, ColorRgba.White, Texture.DualityIconB));
			ContentProvider.AddContent(ContentPath_DualityLogoBig, new Material(DrawTechnique.Alpha, ColorRgba.White, Texture.DualityLogoBig));
			ContentProvider.AddContent(ContentPath_DualityLogoMedium, new Material(DrawTechnique.Alpha, ColorRgba.White, Texture.DualityLogoMedium));
			ContentProvider.AddContent(ContentPath_DualityLogoSmall, new Material(DrawTechnique.Alpha, ColorRgba.White, Texture.DualityLogoSmall));
			ContentProvider.AddContent(ContentPath_Checkerboard, new Material(DrawTechnique.Solid, ColorRgba.White, Texture.Checkerboard));

			SolidWhite			= ContentProvider.RequestContent<Material>(ContentPath_SolidWhite);
			SolidBlack			= ContentProvider.RequestContent<Material>(ContentPath_SolidBlack);
			InvertWhite			= ContentProvider.RequestContent<Material>(ContentPath_InvertWhite);
			DualityIcon			= ContentProvider.RequestContent<Material>(ContentPath_DualityIcon);
			DualityIconB		= ContentProvider.RequestContent<Material>(ContentPath_DualityIconB);
			DualityLogoBig		= ContentProvider.RequestContent<Material>(ContentPath_DualityLogoBig);
			DualityLogoMedium	= ContentProvider.RequestContent<Material>(ContentPath_DualityLogoMedium);
			DualityLogoSmall	= ContentProvider.RequestContent<Material>(ContentPath_DualityLogoSmall);
			Checkerboard		= ContentProvider.RequestContent<Material>(ContentPath_Checkerboard);
		}

		/// <summary>
		/// Creates a new Material Resource based on the specified Texture, saves it and returns a reference to it.
		/// </summary>
		/// <param name="baseRes"></param>
		/// <returns></returns>
		public static ContentRef<Material> CreateFromTexture(ContentRef<Texture> baseRes)
		{
			string resPath = PathHelper.GetFreePath(baseRes.FullName, FileExt);
			Material res = new Material(DrawTechnique.Mask, ColorRgba.White, baseRes);
			res.Save(resPath);
			return res;
		}


		private BatchInfo info = new BatchInfo();
		
		/// <summary>
		/// [GET] Returns a new <see cref="BatchInfo"/> object that mirrors the Materials current settings.
		/// </summary>
		public BatchInfo Info
		{
			get { return new BatchInfo(this.info); }
		}
		/// <summary>
		/// [GET] Returns the Materials internal <see cref="BatchInfo"/> instance that is used for rendering and holds its actual data.
		/// </summary>
		internal BatchInfo InfoDirect
		{
			get { return this.info; }
		}

		/// <summary>
		/// [GET / SET] The <see cref="Duality.Resources.DrawTechnique"/> that is used.
		/// </summary>
		public ContentRef<DrawTechnique> Technique
		{
			get { return this.info.Technique; }
			set { this.info.Technique = value; }
		}
		/// <summary>
		/// [GET / SET] The main color, typically used for coloring displayed vertices.
		/// </summary>
		public ColorRgba MainColor
		{
			get { return this.info.MainColor; }
			set { this.info.MainColor = value; }
		}
		/// <summary>
		/// [GET / SET] The set of <see cref="Duality.Resources.Texture">Textures</see> to use.
		/// </summary>
		public IEnumerable<KeyValuePair<string,ContentRef<Texture>>> Textures
		{
			get { return this.info.Textures; }
			set { this.info.Textures = value; }
		}
		/// <summary>
		/// [GET / SET] The Materials main texture.
		/// </summary>
		public ContentRef<Texture> MainTexture
		{
			get { return this.info.MainTexture; }
			set { this.info.MainTexture = value; }
		}
		/// <summary>
		/// [GET / SET] The set of <see cref="Duality.Resources.ShaderFieldInfo">uniform values</see> to use.
		/// </summary>
		public IEnumerable<KeyValuePair<string,float[]>> Uniforms
		{
			get { return this.info.Uniforms; }
			set { this.info.Uniforms = value; }
		}

		/// <summary>
		/// Creates a new Material
		/// </summary>
		public Material()
		{
			this.info = new BatchInfo();
		}
		/// <summary>
		/// Creates a new single-texture Material.
		/// </summary>
		/// <param name="technique">The <see cref="Duality.Resources.DrawTechnique"/> to use.</param>
		/// <param name="mainColor">The <see cref="MainColor"/> to use.</param>
		/// <param name="mainTex">The main <see cref="Duality.Resources.Texture"/> to use.</param>
		public Material(ContentRef<DrawTechnique> technique, ColorRgba mainColor, ContentRef<Texture> mainTex)
		{
			this.info = new BatchInfo(technique, mainColor, mainTex);
		}
		/// <summary>
		/// Creates a new complex Material.
		/// </summary>
		/// <param name="technique">The <see cref="Duality.Resources.DrawTechnique"/> to use.</param>
		/// <param name="mainColor">The <see cref="MainColor"/> to use.</param>
		/// <param name="textures">A set of <see cref="Duality.Resources.Texture">Textures</see> to use.</param>
		/// <param name="uniforms">A set of <see cref="Duality.Resources.ShaderFieldInfo">uniform values</see> to use.</param>
		public Material(ContentRef<DrawTechnique> technique, ColorRgba mainColor, Dictionary<string,ContentRef<Texture>> textures = null, Dictionary<string,float[]> uniforms = null)
		{
			this.info = new BatchInfo(technique, mainColor, textures, uniforms);
		}
		/// <summary>
		/// Creates a new Material based on the specified BatchInfo
		/// </summary>
		/// <param name="info"></param>
		public Material(BatchInfo info)
		{
			this.info = new BatchInfo(info);
		}
		
		/// <summary>
		/// Gets a texture by name. Returns a null reference if the name doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public ContentRef<Texture> GetTexture(string name)
		{
			return this.info.GetTexture(name);
		}
		/// <summary>
		/// Sets a texture within the Material.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="tex"></param>
		public void SetTexture(string name, ContentRef<Texture> tex)
		{
			this.info.SetTexture(name, tex);
		}
		/// <summary>
		/// Gets a uniform by name. Returns a null reference if the name doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public float[] GetUniform(string name)
		{
			return this.info.GetUniform(name);
		}
		/// <summary>
		/// Sets a uniform value within the Material.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="uniform"></param>
		public void SetUniform(string name, params float[] uniform)
		{
			this.info.SetUniform(name, uniform);
		}
		/// <summary>
		/// Sets a uniform value within the Material.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="index"></param>
		/// <param name="uniformVal"></param>
		public void SetUniform(string name, int index, float uniformVal)
		{
			this.info.SetUniform(name, index, uniformVal);
		}

		protected override void OnLoaded()
		{
			base.OnLoaded();
			// Make references available
			if (this.info != null) this.info.MakeAvailable();
		}
	}
}
