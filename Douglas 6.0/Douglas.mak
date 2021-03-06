# Warning! This file has been generated by the BlackBerry Java Development Environment. Do not modify by hand.
all: \
     Douglas.cod \


clean:
	@if exist Douglas.cod erase Douglas.cod > nul
	@if exist Douglas.lst erase Douglas.lst > nul
	@if exist Douglas.debug erase Douglas.debug > nul
	@if exist Douglas.csl erase Douglas.csl > nul
	@if exist Douglas.cso erase Douglas.cso > nul
	@if exist Douglas.wts erase Douglas.wts > nul
	@if exist Douglas-*.lst erase Douglas-*.lst > nul
	@if exist Douglas-*.debug erase Douglas-*.debug > nul

rebuild: clean all

Private: \


Private_files:


Debug: \


Debug_files:


Release: \
    Douglas.cod \


Release_files:
	@echo "D:\RDouglasFiles\Douglas 6.0\Douglas"


Douglas_sources = \
    com\douglas\common\Angebote.java \
    com\douglas\common\BaseXMLParser.java \
    com\douglas\common\CheckGPS.java \
    com\douglas\common\DouglasConstants.java \
    com\douglas\common\DouglasXMLParser.java \
    com\douglas\common\Event.java \
    com\douglas\common\GPS.java \
    com\douglas\common\LabelListener.java \
    com\douglas\common\LooksTVSpots.java \
    com\douglas\common\MyLabelListener.java \
    com\douglas\common\NetworkUtilities.java \
    com\douglas\common\Neuheiten.java \
    com\douglas\common\NumberFormat.java \
    com\douglas\common\PNGEncoder.java \
    com\douglas\common\Shop.java \
    com\douglas\common\Store.java \
    com\douglas\common\TableLayoutManager.java \
    com\douglas\common\TopTen.java \
    com\douglas\common\TouchBitmapField.java \
    com\douglas\common\TouchBitmapMenuField.java \
    com\douglas\common\WebBitmapField.java \
    com\douglas\common\WebDataCallback.java \
    com\douglas\common\WebVerticalFieldManager.java \
    com\douglas\common\XmlParser.java \
    com\douglas\main\AdvertisePicture.java \
    com\douglas\main\AdvertiseScreen.java \
    com\douglas\main\BaseButtonField.java \
    com\douglas\main\DouglasMenu.java \
    com\douglas\main\EventMaternusScreen.java \
    com\douglas\main\EventsScreenDetail.java \
    com\douglas\main\EventsTabScreen.java \
    com\douglas\main\finder.java \
    com\douglas\main\ForegroundManager.java \
    com\douglas\main\GPSScreen.java \
    com\douglas\main\NegativeMarginVerticalFieldManager.java \
    com\douglas\main\NeuheitenDetailScreen.java \
    com\douglas\main\NeuheitenScreen.java \
    com\douglas\main\PerfumerieScreen.java \
    com\douglas\main\PerfumerieTabScreen.java \
    com\douglas\main\RouteScreen.java \
    com\douglas\main\SearchMapScreen.java \
    com\douglas\main\ShopScreen.java \
    com\douglas\main\SkinTestScreen.java \
    com\douglas\main\startApp.java \
    com\douglas\main\StoreScreen.java \
    com\douglas\main\StoreTabScreen.java \
    com\douglas\main\TabButtonField.java \
    com\douglas\main\TabButtonSet.java \
    com\douglas\main\TopTenDetailScreen.java \
    com\douglas\main\TopTenScreen.java \
    com\douglas\main\TopTenTabScreen.java \
    com\douglas\main\VideoScreen.java \
    com\douglas\main\VideoTabScreen.java \
    com\douglas\main\ZumShop.java \
    com\douglas\utils\Util.java \
    res\drawable\1.png \
    res\drawable\10.png \
    res\drawable\2.png \
    res\drawable\3.png \
    res\drawable\4.png \
    res\drawable\5.png \
    res\drawable\6.png \
    res\drawable\7.png \
    res\drawable\8.png \
    res\drawable\9.png \
    res\drawable\abbrechen_off.png \
    res\drawable\abbrechen_on.png \
    res\drawable\android_magnifier.png \
    res\drawable\arrow_left.png \
    res\drawable\arrow_right.png \
    res\drawable\background_bright.png \
    res\drawable\bbg.png \
    res\drawable\beauty_services.jpg \
    res\drawable\beauty_services_off.png \
    res\drawable\beauty_services_on.png \
    res\drawable\bg_bright.png \
    res\drawable\bottom_bag_off_sprite_new@2x.png \
    res\drawable\chk.jpeg \
    res\drawable\D_bb480x360.png \
    res\drawable\D_bb480x44.png \
    res\drawable\D_text_header.png \
    res\drawable\Default.png \
    res\drawable\douglasheader.png \
    res\drawable\douglasheader@2x.png \
    res\drawable\event_off.png \
    res\drawable\event_on.png \
    res\drawable\events_btn_on.png \
    res\drawable\events_off.png \
    res\drawable\events_on.png \
    res\drawable\handset.png \
    res\drawable\hauttyptest_button.png \
    res\drawable\hauttyptest_button_on.png \
    res\drawable\header_img.png \
    res\drawable\info_off.png \
    res\drawable\info_on.png \
    res\drawable\label_cardexklusiv.gif \
    res\drawable\looks_spots_button.png \
    res\drawable\Looks_TV_button_on.png \
    res\drawable\lupe_44x44_shadow@2x.png \
    res\drawable\lupe_off.png \
    res\drawable\lupe_on.png \
    res\drawable\make-up_vid_off.png \
    res\drawable\make-up_vid_on.png \
    res\drawable\menu_2_off.png \
    res\drawable\menu_2_on.png \
    res\drawable\menu_off.png \
    res\drawable\menu_on.png \
    res\drawable\menub_off.png \
    res\drawable\menub_on.png \
    res\drawable\neu_off.png \
    res\drawable\neu_on.png \
    res\drawable\neue_suche_off.png \
    res\drawable\neue_suche_on.png \
    res\drawable\neuheiten_button.png \
    res\drawable\neuheiten_button_on.png \
    res\drawable\neuheiten_off.png \
    res\drawable\neuheiten_on.png \
    res\drawable\OK_off.png \
    res\drawable\OK_on.png \
    res\drawable\Parf_events_button_on.png \
    res\drawable\parf_mit_event_off.png \
    res\drawable\parf_mit_event_on.png \
    res\drawable\parfumerie_events_button.png \
    res\drawable\parfumerie_off.png \
    res\drawable\parfumerie_on.png \
    res\drawable\parfumerien_off.png \
    res\drawable\parfumerien_on.png \
    res\drawable\phoneIcon@2x.png \
    res\drawable\search.png \
    res\drawable\service.jpg \
    res\drawable\shop_button.png \
    res\drawable\Shop_kachel_on.png \
    res\drawable\shop_off.png \
    res\drawable\shop_on.png \
    res\drawable\suchen_off.png \
    res\drawable\suchen_on.png \
    res\drawable\suchergebnis_off.png \
    res\drawable\suchergebnis_on.png \
    res\drawable\top10_button.png \
    res\drawable\top10_button_on.png \
    res\drawable\top10_damen_off.png \
    res\drawable\top10_damen_on.png \
    res\drawable\top10_herren_off.png \
    res\drawable\top10_herren_on.png \
    res\drawable\top10_off.png \
    res\drawable\top10_on.png \
    res\drawable\tv-spots_off.png \
    res\drawable\tv-spots_on.png \
    res\drawable\video_off.png \
    res\drawable\video_on.png \
    res\drawable\zumshop_off.jpg \
    res\drawable\zumshop_on.jpg \
    res\drawable\zuruck_off.png \
    res\drawable\zuruck_on.png \
    res\layout\ARichTextField.java \
    res\layout\ComboBox.java \
    res\layout\CustomButtonField.java \
    res\layout\CustomPopupScreen.java \
    res\layout\DouglasFooter.java \
    res\layout\EventLabel.java \
    res\layout\HeaderLabel.java \
    res\layout\MenuScreen.java \
    res\layout\PrevNext.java \
    res\layout\RichTextLabel.java \
    res\layout\RouteLabel.java \
    res\layout\StoreHeaderLabel.java \
    res\layout\SubTitle.java \
    res\layout\SubTitleLabel.java \
    res\layout\TelephoneLabel.java \
    res\layout\TextLabel.java \
    res\layout\TitleLabel.java \
    res\layout\VideoPopupScreen.java \

Douglas_dependencies = \
    res\drawable\icon.png \
    "..\..\Research In Motion\BlackBerry JDE 6.0.0\lib\net_rim_api.jar" \
    Douglas.rapc \
    Douglas_build.files \

Douglas.cod : $(Douglas_sources) $(Douglas_dependencies)
	@if exist Douglas.cod erase Douglas.cod > nul
	@if exist Douglas.lst erase Douglas.lst > nul
	@if exist Douglas.debug erase Douglas.debug > nul
	@if exist Douglas.csl erase Douglas.csl > nul
	@if exist Douglas.cso erase Douglas.cso > nul
	@if exist Douglas.wts erase Douglas.wts > nul
	@if exist Douglas-*.lst erase Douglas-*.lst > nul
	@if exist Douglas-*.debug erase Douglas-*.debug > nul
	@echo Building Douglas ...
	D:\Research In Motion\BlackBerry JDE 6.0.0\bin\rapc.exe -quiet codename=Douglas Douglas.rapc warnkey=0x52424200;0x52525400;0x52435200  @Douglas_build.files
	@if exist "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.cod" erase "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.cod" > nul
	@if exist Douglas.cod copy Douglas.cod "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.cod" > nul
	@if exist "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.jar" erase "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.jar" > nul
	@if exist Douglas.jar copy Douglas.jar "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.jar" > nul
	@if exist "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.lst" erase "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.lst" > nul
	@if exist Douglas.lst copy Douglas.lst "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.lst" > nul
	@if exist "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.debug" erase "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.debug" > nul
	@if exist Douglas.debug copy Douglas.debug "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.debug" > nul
	@if exist "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.csl" erase "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.csl" > nul
	@if exist Douglas.csl copy Douglas.csl "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.csl" > nul
	@if exist "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.cso" erase "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.cso" > nul
	@if exist Douglas.cso copy Douglas.cso "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.cso" > nul
	@if exist "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.wts" erase "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.wts" > nul
	@if exist Douglas.wts copy Douglas.wts "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas.wts" > nul
	@if exist "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas-*.debug" erase "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas-*.debug" > nul
	@if exist Douglas-*.debug copy Douglas-*.debug "D:\Research In Motion\BlackBerry JDE 6.0.0\simulator\Douglas-*.debug" > nul


