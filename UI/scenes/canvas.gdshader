shader_type canvas_item;

uniform sampler2D _canvas_data;

void fragment() {
	COLOR = texture(_canvas_data, UV);
}
