// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
using System.Text.Json.Serialization;

namespace CandyKingdom.Marcy.ImageTools;

public record FFProbeStreamData(
        [property: JsonPropertyName("index")] int? Index,
        [property: JsonPropertyName("codec_name")] string CodecName,
        [property: JsonPropertyName("codec_long_name")] string CodecLongName,
        [property: JsonPropertyName("profile")] string Profile,
        [property: JsonPropertyName("codec_type")] string CodecType,
        [property: JsonPropertyName("codec_tag_string")] string CodecTagString,
        [property: JsonPropertyName("codec_tag")] string CodecTag,
        [property: JsonPropertyName("width")] int? Width,
        [property: JsonPropertyName("height")] int? Height,
        [property: JsonPropertyName("coded_width")] int? CodedWidth,
        [property: JsonPropertyName("coded_height")] int? CodedHeight,
        [property: JsonPropertyName("closed_captions")] int? ClosedCaptions,
        [property: JsonPropertyName("film_grain")] int? FilmGrain,
        [property: JsonPropertyName("has_b_frames")] int? HasBFrames,
        [property: JsonPropertyName("sample_aspect_ratio")] string SampleAspectRatio,
        [property: JsonPropertyName("display_aspect_ratio")] string DisplayAspectRatio,
        [property: JsonPropertyName("pix_fmt")] string PixFmt,
        [property: JsonPropertyName("level")] int? Level,
        [property: JsonPropertyName("color_range")] string ColorRange,
        [property: JsonPropertyName("color_space")] string ColorSpace,
        [property: JsonPropertyName("color_transfer")] string ColorTransfer,
        [property: JsonPropertyName("color_primaries")] string ColorPrimaries,
        [property: JsonPropertyName("chroma_location")] string ChromaLocation,
        [property: JsonPropertyName("field_order")] string FieldOrder,
        [property: JsonPropertyName("refs")] int? Refs,
        [property: JsonPropertyName("is_avc")] string IsAvc,
        [property: JsonPropertyName("nal_length_size")] string NalLengthSize,
        [property: JsonPropertyName("id")] string Id,
        [property: JsonPropertyName("r_frame_rate")] string RFrameRate,
        [property: JsonPropertyName("avg_frame_rate")] string AvgFrameRate,
        [property: JsonPropertyName("time_base")] string TimeBase,
        [property: JsonPropertyName("start_pts")] int? StartPts,
        [property: JsonPropertyName("start_time")] string StartTime,
        [property: JsonPropertyName("duration_ts")] int? DurationTs,
        [property: JsonPropertyName("duration")] string Duration,
        [property: JsonPropertyName("bit_rate")] string BitRate,
        [property: JsonPropertyName("bits_per_raw_sample")] string BitsPerRawSample,
        [property: JsonPropertyName("nb_frames")] string NbFrames,
        [property: JsonPropertyName("extradata_size")] int? ExtradataSize,
        [property: JsonPropertyName("tags")] FFProbeMetaTags Tags,
        [property: JsonPropertyName("sample_fmt")] string SampleFmt,
        [property: JsonPropertyName("sample_rate")] string SampleRate,
        [property: JsonPropertyName("channels")] int? Channels,
        [property: JsonPropertyName("channel_layout")] string ChannelLayout,
        [property: JsonPropertyName("bits_per_sample")] int? BitsPerSample,
        [property: JsonPropertyName("initial_padding")] int? InitialPadding
    );

public record FFProbeMetaFormat(
        [property: JsonPropertyName("filename")] string Filename,
        [property: JsonPropertyName("nb_streams")] int NbStreams,
        [property: JsonPropertyName("nb_programs")] int NbPrograms,
        [property: JsonPropertyName("format_name")] string FormatName,
        [property: JsonPropertyName("format_long_name")] string FormatLongName,
        [property: JsonPropertyName("start_time")] string StartTime,
        [property: JsonPropertyName("duration")] string Duration,
        [property: JsonPropertyName("size")] string Size,
        [property: JsonPropertyName("bit_rate")] string BitRate,
        [property: JsonPropertyName("probe_score")] int ProbeScore,
        [property: JsonPropertyName("tags")] FFProbeMetaTags Tags
    );

public record FFProbeMeta(
    [property: JsonPropertyName("streams")] IReadOnlyList<FFProbeStreamData> Streams,
    [property: JsonPropertyName("format")] FFProbeMetaFormat Format
);

public record FFProbeMetaTags(
    [property: JsonPropertyName("major_brand")] string MajorBrand,
    [property: JsonPropertyName("minor_version")] string MinorVersion,
    [property: JsonPropertyName("compatible_brands")] string CompatibleBrands,
    [property: JsonPropertyName("encoder")] string Encoder
);
