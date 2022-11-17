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
	if (priceInputMax != null) {
		priceInputMax.addEventListener('input', function () {
			if (priceInputMax.value > 999000) {

				priceInputMax.value = 999000;
			}
			if (priceInputMax.value < 1000) {
				
				priceInputMax.value = 1000;
			}
		})
	}
	if (priceInputMin != null) {
		priceInputMin.addEventListener('input', function () {
			if (priceInputMin.value > 999000) {
				
				priceInputMin.value = 999000;
			}
			if (priceInputMin.value < 1000) {

				priceInputMin.value = 1000;
			}
		});
	}
	if (priceInputMax != null) {
		priceInputMax.addEventListener('change', function () {
			
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
    }

	if (priceInputMin != null) {
		priceInputMin.addEventListener('change', function () {

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
	}
	//thêm mới vào giỏ hàng
	$(".add-to-cart-btn").click(function () {

		var idsp = $(this).attr("value");
		$.post("/GioHang/AddToCart",
			{
				id: idsp,
				SoLuong: 1
			},
			function (data) {
				if (data == "Số lượng không có sẵn") {
					$('.messErorr').html('<div class="alert alert-danger text-center" role="alert"> <i class="fa fa-exclamation-triangle" aria-hidden="true"></i>' + data + '</div >');
                } else {
					$('.messCart').append(data);
                }
				
				setTimeout(function () {
					location.reload();
				}, 1000)

			});
	});
	
	//Sửa số lượng cart
	//$('#numberCart').change(function () {
	//	let idProduct = $(this).attr("data-idProduct");
	//	let soLuongmoi = numberCart.value;
	//	$.post("/GioHang/SuaSoLuong",
	//		{
	//			id: idProduct,
	//			soluongmoi: soLuongmoi
	//		},
	//		function (data) {

	//			if (data.mess == null) {
				
	//				location.reload();

	//			} else {
	//				$('#messNumberCart').text(data.mess);
					
	//				setTimeout(function () {
	//					location.reload();
	//				}, 1000)
					
	//			}

	//		});
	//});

	//$("#CartUpdateMax").click(function () {

	//	let idProduct = $(this).attr("data-idProduct");
	
	//	let soLuong = $(this).attr("data-soLuong");
	//	$.post("/GioHang/SuaSoLuong2",
	//		{
	//			id: idProduct,
				
	//			soLuong: soLuong
	//		},
	//		function () {
				
	//				location.reload();
				

	//		});



	//});
	//$("#CartUpdateMin").click(function () {

	//	let idProduct = $(this).attr("data-idProduct");
		
	//	$.post("/GioHang/SuaSoLuong2",
	//		{
	//			id: idProduct,
				
	//			soLuong: -1
	//		},
	//		function () {

	//			location.reload();


	//		});



	//});

	//$('#numberCart').on("input", function () {
		
	//	if (numberCart.value > 100) {

	//		numberCart.value = 100;
	//	}
	//	if (numberCart.value < 1) {

	//		numberCart.value = 1;
	//	}
	//});






	$('.dropdown').hover(function () {
		$(this).toggleClass('open');
	});

	

	


})(jQuery);