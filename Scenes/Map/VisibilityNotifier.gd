extends Sprite

onready var visibility_notifier := $VisibilityNotifier2D

func _ready() -> void:
	var enterErr = visibility_notifier.connect("screen_entered", self, "show")
	var exitErr = visibility_notifier.connect("screen_exited", self, "hide")
	if enterErr != OK || exitErr != exitErr:
		print("VisibilityNotifier Error")
	visible = false
