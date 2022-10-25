(function ($) {
	"use strict"

	// Mobile Nav toggle
	$('.menu-toggle > a').on('click', function (e) {
		e.preventDefault();
		$('#responsive-nav').toggleClass('active');
	})

	// Fix cart dropdown from closing
	$('.cart-dropdown').on('click', function (e) {
		e.stopPropagation();
	});

	/////////////////////////////////////////

	// Products Slick
	$('.products-slick').each(function () {
		var $this = $(this),
			$nav = $this.attr('data-nav');

		$this.slick({
			slidesToShow: 4,
			slidesToScroll: 1,
			autoplay: true,
			infinite: true,
			speed: 300,
			dots: false,
			arrows: true,
			appendArrows: $nav ? $nav : false,
			responsive: [{
				breakpoint: 991,
				settings: {
					slidesToShow: 2,
					slidesToScroll: 1,
				}
			},
			{
				breakpoint: 480,
				settings: {
					slidesToShow: 1,
					slidesToScroll: 1,
				}
			},
			]
		});
	});

	// Products Widget Slick
	$('.products-widget-slick').each(function () {
		var $this = $(this),
			$nav = $this.attr('data-nav');

		$this.slick({
			infinite: true,
			autoplay: true,
			speed: 300,
			dots: false,
			arrows: true,
			appendArrows: $nav ? $nav : false,
		});
	});

	/////////////////////////////////////////

	// Product Main img Slick
	$('#product-main-img').slick({
		infinite: true,
		speed: 300,
		dots: false,
		arrows: true,
		fade: true,
		asNavFor: '#product-imgs',
	});

	// Product imgs Slick
	$('#product-imgs').slick({
		slidesToShow: 3,
		slidesToScroll: 1,
		arrows: true,
		centerMode: true,
		focusOnSelect: true,
		centerPadding: 0,
		vertical: true,
		asNavFor: '#product-main-img',
		responsive: [{
			breakpoint: 991,
			settings: {
				vertical: false,
				arrows: false,
				dots: true,
			}
		},
		]
	});

	// Product img zoom
	var zoomMainProduct = document.getElementById('product-main-img');
	if (zoomMainProduct) {
		$('#product-main-img .product-preview').zoom();
	}

	/////////////////////////////////////////

	// Input number
	$('.input-number').each(function () {
		var $this = $(this),
			$input = $this.find('input[type="number"]'),
			up = $this.find('.qty-up'),
			down = $this.find('.qty-down');

		down.on('click', function () {
			var value = parseInt($input.val()) - 1;
			value = value < 1 ? 1 : value;
			$input.val(value);
			$input.change();

		})

		up.on('click', function () {
			var value = parseInt($input.val()) + 1;
			$input.val(value);
			$input.change();

		})
	});

	let priceInputMax = document.getElementById('price-max');
	let priceInputMin = document.getElementById('price-min');

	priceInputMax.addEventListener('input', function () {
		if (priceInputMax.value > 999) {

			priceInputMax.value = 999;
		}
		if (priceInputMax.value < 1) {
			priceInputMax.style.borderColor = "red";
			priceInputMax.value = 1;
		}
	})
	priceInputMin.addEventListener('input', function () {
		if (priceInputMin.value > 999) {
			priceInputMin.style.borderColor = "red";
			priceInputMin.value = 999;
		}
		if (priceInputMin.value < 1) {

			priceInputMin.value = 1;
		}
	});

	priceInputMax.addEventListener('change', function () {
		/*updatePriceSlider($(this).parent(), this.value);*/
		let search = $(this).attr("data-search");
		let currentFilter = "";
		let iddanhmuc = $(this).attr("data-iddanhmuc");
		let idtheloai = $(this).attr("data-idtheloai");
		let idtacgia = $(this).attr("data-idtacgia");
		let sortOrder = $(this).attr("data-sortOrder");
		let pageNumber = $(this).attr("data-pageNumber");
		let pageSize = $(this).attr("data-pageSize");

		let priceMax = priceInputMax.value;
		let priceMin = priceInputMin.value;




		window.location.href = `https://localhost:7047/Home/Product?search=${search}&currentFilter=${currentFilter}&iddanhmuc=${iddanhmuc}&idtheloai=${idtheloai}&idtacgia=${idtacgia}&sortOrder=${sortOrder}&pageNumber=${pageNumber}&pageSize=${pageSize}&priceMax=${priceMax}&priceMin=${priceMin}`

	});

	priceInputMin.addEventListener('change', function () {
		/*updatePriceSlider($(this).parent(), this.value);*/
		let search = $(this).attr("data-search");
		let currentFilter = "";
		let iddanhmuc = $(this).attr("data-iddanhmuc");
		let idtheloai = $(this).attr("data-idtheloai");
		let idtacgia = $(this).attr("data-idtacgia");
		let sortOrder = $(this).attr("data-sortOrder");
		let pageNumber = $(this).attr("data-pageNumber");
		let pageSize = $(this).attr("data-pageSize");

		let priceMax = priceInputMax.value;
		let priceMin = priceInputMin.value;
		window.location.href = `https://localhost:7047/Home/Product?search=${search}&currentFilter=${currentFilter}&iddanhmuc=${iddanhmuc}&idtheloai=${idtheloai}&idtacgia=${idtacgia}&sortOrder=${sortOrder}&pageNumber=${pageNumber}&pageSize=${pageSize}&priceMax=${priceMax}&priceMin=${priceMin}`

	});

	//function updatePriceSlider(elem , value) {
	//	if ( elem.hasClass('price-min') ) {
	//		console.log('min')
	//		priceSlider.noUiSlider.set([value, null]);
	//	} else if ( elem.hasClass('price-max')) {
	//		console.log('max')
	//		priceSlider.noUiSlider.set([null, value]);
	//	}
	//}

	//// Price Slider
	//var priceSlider = document.getElementById('price-slider');
	//if (priceSlider) {
	//	noUiSlider.create(priceSlider, {
	//		start: [1, 999],
	//		connect: true,
	//		step: 1,
	//		range: {
	//			'min': 1,
	//			'max': 999
	//		}
	//	});

	//	priceSlider.noUiSlider.on('update', function( values, handle ) {
	//		var value = values[handle];
	//		handle ? priceInputMax.value = value : priceInputMin.value = value



	//	});
	//	//priceSlider.addEventListener('click', function (values, handle) {
	//	//	var value = values[handle];
	//	//	handle ? priceInputMax.value = value : priceInputMin.value = value
	//	//	window.location.href = `https://localhost:7047/Home/Product?search=${search}&currentFilter=${currentFilter}&iddanhmuc=${iddanhmuc}&idtheloai=${idtheloai}&idtacgia=${idtacgia}&sortOrder=${sortOrder}&pageNumber=${pageNumber}&pageSize=${pageSize}&priceMax=${priceMax}&priceMin=${priceMin}`
	//	//	alert("xxx")


	//	//});



	//}


})(jQuery);