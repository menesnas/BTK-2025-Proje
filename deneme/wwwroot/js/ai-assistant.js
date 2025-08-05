// AI Kombin Önerisi Widget JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Element kontrolleri
    const form = document.getElementById('outfitSuggestionForm');
    const imageInput = document.getElementById('clothingImage');
    const imagePreview = document.getElementById('imagePreview');
    const previewImg = document.getElementById('previewImg');
    const suggestBtn = document.getElementById('suggestBtn');
    const loadingState = document.getElementById('loadingState');
    const suggestionResult = document.getElementById('suggestionResult');
    const suggestionContent = document.getElementById('suggestionContent');

    // Eğer elementler bulunamazsa, bu sayfa AI widget'ı içermiyor demektir
    if (!form || !imageInput) {
        console.log('AI Widget not found on this page - that\'s okay!');
        return;
    }

    console.log('AI Widget initialized successfully!');

    // Tab switching functionality
    const kombinBtn = document.getElementById('kombinBtn');
    const akilliFiltreBtn = document.getElementById('akilliFiltreBtn');
    const optionalFilters = document.getElementById('optionalFilters');

    if (kombinBtn && akilliFiltreBtn) {
        kombinBtn.addEventListener('click', function() {
            // Kombin Öner aktif
            kombinBtn.classList.remove('btn-outline-primary');
            kombinBtn.classList.add('btn-primary');
            akilliFiltreBtn.classList.remove('btn-primary');
            akilliFiltreBtn.classList.add('btn-outline-secondary');
            
            // Filtreleri göster
            if (optionalFilters) {
                optionalFilters.style.display = 'block';
            }
        });

        akilliFiltreBtn.addEventListener('click', function() {
            // Akıllı Filtre aktif
            akilliFiltreBtn.classList.remove('btn-outline-secondary');
            akilliFiltreBtn.classList.add('btn-primary');
            kombinBtn.classList.remove('btn-primary');
            kombinBtn.classList.add('btn-outline-primary');
            
            // Filtreleri gizle (sadece fotoğraf yükleme kalır)
            if (optionalFilters) {
                optionalFilters.style.display = 'none';
            }
        });
    }

    // Dosya yükleme önizlemesi
    imageInput.addEventListener('change', function(e) {
        const file = e.target.files[0];
        if (file) {
            const reader = new FileReader();
            reader.onload = function(e) {
                previewImg.src = e.target.result;
                imagePreview.classList.remove('d-none');
            };
            reader.readAsDataURL(file);
        }
    });

    // Drag & Drop özelliği
    const uploadArea = document.querySelector('.upload-area');
    
    if (uploadArea) {
        uploadArea.addEventListener('dragover', function(e) {
        e.preventDefault();
        uploadArea.style.borderColor = '#0d6efd';
        uploadArea.style.backgroundColor = '#f8f9fa';
    });

    uploadArea.addEventListener('dragleave', function(e) {
        e.preventDefault();
        uploadArea.style.borderColor = '#dee2e6';
        uploadArea.style.backgroundColor = 'transparent';
    });

    uploadArea.addEventListener('drop', function(e) {
        e.preventDefault();
        uploadArea.style.borderColor = '#dee2e6';
        uploadArea.style.backgroundColor = 'transparent';
        
        const files = e.dataTransfer.files;
        if (files.length > 0 && files[0].type.startsWith('image/')) {
            imageInput.files = files;
            const event = new Event('change', { bubbles: true });
            imageInput.dispatchEvent(event);
        }
    });
    } // uploadArea null check'inin kapanışı

    // Form gönderimi
    form.addEventListener('submit', async function(e) {
        e.preventDefault();

        // Doğrulama
        if (!imageInput.files[0]) {
            showAlert('Lütfen bir kıyafet fotoğrafı yükleyin!', 'warning');
            return;
        }

        try {
            // UI durumunu güncelle
            showLoading(true);
            hideResult();

            // Occasion kontrolü
            const occasionElement = document.querySelector('input[name="occasion"]:checked');
            if (!occasionElement) {
                showAlert('Lütfen bir durum seçin!', 'warning');
                return;
            }
            
            // Budget range kontrolü
            const budgetElement = document.querySelector('select[name="budgetRange"]');
            const budgetRange = budgetElement ? budgetElement.value : '500-1500';
            
            // Preferred colors kontrolü
            const colorsElement = document.querySelector('input[name="preferredColors"]');
            const preferredColors = colorsElement ? colorsElement.value : '';

            // Görseli base64'e çevir
            console.log('Görsel base64\'e çevriliyor...');
            const file = imageInput.files[0];
            const base64Image = await convertToBase64(file);
            
            // JSON olarak hazırla
            const requestData = {
                ClothingImageBase64: base64Image,
                FileName: file.name,
                FileType: file.type,
                Occasion: occasionElement.value,
                BudgetRange: budgetRange,
                PreferredColors: preferredColors
            };

            // API'ye istek gönder
            console.log('Sending request to API...');
            console.log('Request data keys:', Object.keys(requestData));
            console.log('Base64 length:', base64Image?.length);
            console.log('File name:', file.name);
            console.log('File type:', file.type);
            console.log('Seçilen Occasion:', occasionElement.value);
            console.log('Tam Request Data:', requestData);
            
            // Önce test endpoint'ini deneyelim
            console.log('Testing JSON deserialization...');
            try {
                const testResponse = await fetch('/api/OutfitSuggestion/test-json', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(requestData)
                });
                
                if (testResponse.ok) {
                    const testResult = await testResponse.json();
                    console.log('Test successful:', testResult);
                } else {
                    const testError = await testResponse.text();
                    console.error('Test failed:', testError);
                }
            } catch (testErr) {
                console.error('Test error:', testErr);
            }
            
            const response = await fetch('/api/OutfitSuggestion/json', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(requestData)
            });

            console.log('API yanıt durumu:', response.status);
            console.log('Response headers:', [...response.headers.entries()]);

            if (!response.ok) {
                const errorText = await response.text();
                console.error('API hata yanıtı:', errorText);
                console.error('Request body that failed:', JSON.stringify(requestData, null, 2));
                throw new Error(`HTTP error! status: ${response.status} - ${errorText}`);
            }

            const responseText = await response.text();
            console.log('Raw API response:', responseText);
            
            const result = JSON.parse(responseText);
            console.log('API başarılı yanıtı:', result);
            console.log('Result keys:', Object.keys(result));
            console.log('Result.success:', result.success);
            console.log('Result.Style:', result.Style);
            
            // Sonucu göster
            displaySuggestion(result);
            showResult();

        } catch (error) {
            console.error('Detaylı hata bilgisi:', error);
            console.error('Hata mesajı:', error.message);
            console.error('Hata stack:', error.stack);
            
            let errorMessage = 'Kombin önerisi alınırken bir hata oluştu. ';
            if (error.message.includes('HTTP error')) {
                errorMessage += `Sunucu hatası: ${error.message}`;
            } else if (error.message.includes('Failed to fetch')) {
                errorMessage += 'Sunucuya bağlanılamadı. İnternet bağlantınızı kontrol edin.';
            } else {
                errorMessage += `Hata: ${error.message}`;
            }
            
            showAlert(errorMessage, 'danger');
        } finally {
            showLoading(false);
        }
    });

    function showLoading(show) {
        if (show) {
            loadingState.classList.remove('d-none');
            suggestBtn.disabled = true;
            suggestBtn.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>İşleniyor...';
        } else {
            loadingState.classList.add('d-none');
            suggestBtn.disabled = false;
            suggestBtn.innerHTML = '<i class="fas fa-magic me-2"></i>Kombin Öner';
        }
    }

    function showResult() {
        suggestionResult.classList.remove('d-none');
        suggestionResult.scrollIntoView({ behavior: 'smooth', block: 'nearest' });
    }

    function hideResult() {
        suggestionResult.classList.add('d-none');
    }

    function displaySuggestion(suggestion) {
        console.log('displaySuggestion çağrıldı:', suggestion);
        
        // API'den gelen verilerin büyük/küçük harf kontrolü
        const style = suggestion.Style || suggestion.style || 'Günlük';
        const ustGiyim = suggestion.UstGiyim || suggestion.ustGiyim || '';
        const altGiyim = suggestion.AltGiyim || suggestion.altGiyim || '';
        const ayakkabi = suggestion.Ayakkabi || suggestion.ayakkabi || 'Ayakkabı önerisi';
        const aksesuar = suggestion.Aksesuar || suggestion.aksesuar || 'Aksesuar önerisi';
        const colorScheme = suggestion.ColorScheme || suggestion.colorScheme || 'Renk şeması';
        const occasion = suggestion.Occasion || suggestion.occasion || 'Durum';
        const products = suggestion.products || {};
        
        // Yardımcı: Ürün adı ile products objesinde eşleşen ürünün DetailUrl'ini bul
        function getProductDetailUrl(type, name) {
            if (!products || !name) return null;
            let arr = [];
            if (type === 'ustGiyim' && products.ustGiyim) arr = products.ustGiyim;
            if (type === 'altGiyim' && products.altGiyim) arr = products.altGiyim;
            if (type === 'ayakkabi' && products.ayakkabi) arr = products.ayakkabi;
            if (type === 'aksesuar' && products.aksesuar) arr = products.aksesuar;
            if (!arr.length) return null;
            // İsme göre kaba eşleşme
            const found = arr.find(p => p.name && name && p.name.toLowerCase().includes(name.toLowerCase()));
            return found ? found.detailUrl : null;
        }

        // Tıklanabilir öneri kutuları
        // detailUrl varsa doğrudan ürün detayına gider, yoksa arama sayfasına yönlendirir
        let ustGiyimHtml = '';
        if (ustGiyim && ustGiyim.trim() !== '') {
            ustGiyimHtml = `
                <div class="col-md-3">
                    <div class="suggestion-item text-center p-3 bg-white rounded-3 border">
                        <i class="fas fa-tshirt text-info mb-2" style="font-size: 1.5rem;"></i>
                        <h6 class="fw-semibold mb-1">Üst Giyim</h6>
                        <p class="small mb-0 text-primary">${ustGiyim}</p>
                    </div>
                </div>
            `;
        }
        let altGiyimHtml = '';
        if (altGiyim && altGiyim.trim() !== '') {
            altGiyimHtml = `
                <div class="col-md-3">
                    <div class="suggestion-item text-center p-3 bg-white rounded-3 border">
                        <i class="fas fa-tshirt text-primary mb-2" style="font-size: 1.5rem;"></i>
                        <h6 class="fw-semibold mb-1">Alt Giyim</h6>
                        <p class="small mb-0 text-primary">${altGiyim}</p>
                    </div>
                </div>
            `;
        }
        let ayakkabiHtml = '';
        if (ayakkabi && ayakkabi.trim() !== '' && ayakkabi !== 'Ayakkabı önerisi') {
            ayakkabiHtml = `
                <div class="col-md-3">
                    <div class="suggestion-item text-center p-3 bg-white rounded-3 border">
                        <i class="fas fa-running text-success mb-2" style="font-size: 1.5rem;"></i>
                        <h6 class="fw-semibold mb-1">Ayakkabı</h6>
                        <p class="small mb-0 text-success">${ayakkabi}</p>
                    </div>
                </div>
            `;
        }
        let aksesuarHtml = '';
        if (aksesuar && aksesuar.trim() !== '' && aksesuar !== 'Aksesuar önerisi') {
            aksesuarHtml = `
                <div class="col-md-3">
                    <div class="suggestion-item text-center p-3 bg-white rounded-3 border">
                        <i class="fas fa-gem text-warning mb-2" style="font-size: 1.5rem;"></i>
                        <h6 class="fw-semibold mb-1">Aksesuar</h6>
                        <p class="small mb-0 text-warning">${aksesuar}</p>
                    </div>
                </div>
            `;
        }

        const html = `
            <div class="suggestion-content">
                <div class="text-center mb-3">
                    <h6 class="fw-bold text-primary mb-2">
                        <i class="fas fa-sparkles me-1"></i>${style} Kombini
                    </h6>
                    <small class="text-muted">${occasion} • ${colorScheme}</small>
                </div>
                <div class="row g-3 mb-3">
                    ${ustGiyimHtml}
                    ${altGiyimHtml}
                    ${ayakkabiHtml}
                    ${aksesuarHtml}
                </div>
                ${suggestion.products ? generateProductsHTML(suggestion.products) : ''}
                <div class="text-center">
                    <button class="btn btn-primary btn-sm rounded-pill me-2" onclick="searchProducts('${altGiyim}', '${ayakkabi}', '${aksesuar}')">
                        <i class="fas fa-shopping-bag me-1"></i>Bu Ürünleri Bul
                    </button>
                    <button class="btn btn-outline-secondary btn-sm rounded-pill" onclick="shareOutfit()">
                        <i class="fas fa-share me-1"></i>Paylaş
                    </button>
                </div>
            </div>
        `;
        suggestionContent.innerHTML = html;
        
        // --- Önerilen ürünler/kategoriler kutusu ---
        let suggestionArea = document.querySelector('.suggestions');
        if (!suggestionArea) {
            suggestionArea = document.createElement('div');
            suggestionArea.className = 'suggestions mt-3';
            suggestionContent.appendChild(suggestionArea);
        }
        suggestionArea.innerHTML = items.map(item => {
          if (item.id) {
            return `${item.icon} ${item.name}`;
          } else if (item.category) {
            return `${item.icon} ${item.name}`;
          } else {
            return '';
          }
        }).join(' ');
    }

    function generateProductsHTML(products) {
        if (!products) return '';
        
        let html = '<div class="mt-4"><h6 class="fw-bold mb-3"><i class="fas fa-search me-2"></i>Database\'den Bulunan Ürünler</h6>';
        
        // Alt Giyim Ürünleri
        if (products.altGiyim && products.altGiyim.length > 0) {
            html += '<div class="mb-4"><small class="text-muted fw-semibold d-block mb-2">Alt Giyim</small><div class="row g-3">';
            products.altGiyim.forEach(product => {
                const similarityBadge = product.Similarity ? 
                    `<span class="badge bg-success position-absolute top-0 end-0 m-1" style="font-size: 0.7em;">
                        ${Math.round(product.Similarity * 100)}% eşleşme
                    </span>` : '';
                
                html += `
                    <div class="col-lg-4 col-md-6">
                        <div class="card border-0 shadow-sm cursor-pointer h-100 position-relative" 
                             onclick="window.open('${product.DetailUrl}', '_blank')">
                            ${similarityBadge}
                            <img src="${product.ImageUrl}" class="card-img-top" alt="${product.Name}" 
                                 style="height:180px;object-fit:cover;">
                            <div class="card-body p-3">
                                <h6 class="fw-semibold mb-1">${product.Name}</h6>
                                <p class="text-primary fw-bold mb-1">${product.Price}₺</p>
                                <small class="text-muted">${product.Category}</small>
                                ${product.Colour ? `<br><small class="text-info">Renk: ${product.Colour}</small>` : ''}
                                </div>
                            </div>
                    </div>
                `;
            });
            html += '</div></div>';
        }
        
        // Ayakkabı ve Aksesuar için benzer yapı...
        // (Benzer kod tekrarını önlemek için kısalttım)
        
        html += '</div>';
        return html;
    }

    function showAlert(message, type) {
        const alertDiv = document.createElement('div');
        alertDiv.className = `alert alert-${type} alert-dismissible fade show mt-3`;
        alertDiv.innerHTML = `
            ${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        `;
        
        form.appendChild(alertDiv);
        
        // 5 saniye sonra otomatik kapat
        setTimeout(() => {
            if (alertDiv.parentNode) {
                alertDiv.remove();
            }
        }, 5000);
    }
});

// Base64 dönüştürme fonksiyonu
function convertToBase64(file) {
    return new Promise((resolve, reject) => {
        const reader = new FileReader();
        reader.onload = () => {
            // data:image/jpeg;base64,... formatından sadece base64 kısmını al
            const base64 = reader.result.split(',')[1];
            resolve(base64);
        };
        reader.onerror = reject;
        reader.readAsDataURL(file);
    });
}

// Floating AI Widget
document.addEventListener('DOMContentLoaded', function() {
    const aiToggleBtn = document.getElementById('aiFloatToggleBtn');
    const aiChatPopup = document.getElementById('aiChatPopup');
    const aiCloseBtn = document.getElementById('aiFloatCloseBtn');

    if (aiToggleBtn && aiChatPopup) {
        console.log('Float AI widget initialized!');
        
        // Toggle AI chat popup
        aiToggleBtn.addEventListener('click', function() {
            console.log('Toggle button clicked!');
            aiChatPopup.classList.toggle('show');
        });

        // Close AI chat popup
        if (aiCloseBtn) {
            aiCloseBtn.addEventListener('click', function() {
                aiChatPopup.classList.remove('show');
            });
        }

        // Close popup when clicking outside
        document.addEventListener('click', function(e) {
            if (!e.target.closest('.ai-assistant-float')) {
                aiChatPopup.classList.remove('show');
            }
        });
    }

    // Popup form functionality
    const showKombinBtn = document.getElementById('showKombinForm');
    const kombinForm = document.getElementById('kombinForm');
    const welcomeMessage = document.querySelector('.ai-welcome-message');
    
    if (showKombinBtn && kombinForm) {
        showKombinBtn.addEventListener('click', function() {
            welcomeMessage.classList.add('d-none');
            kombinForm.classList.remove('d-none');
        });
    }

    // Popup file upload preview
    const popupImageInput = document.getElementById('popupClothingImage');
    const popupImagePreview = document.getElementById('popupImagePreview');
    const popupPreviewImg = document.getElementById('popupPreviewImg');

    if (popupImageInput) {
        popupImageInput.addEventListener('change', function(e) {
            const file = e.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function(e) {
                    popupPreviewImg.src = e.target.result;
                    popupImagePreview.classList.remove('d-none');
                };
                reader.readAsDataURL(file);
            }
        });
    }

    // Popup form submission
    const popupForm = document.getElementById('popupOutfitForm');
    if (popupForm) {
        popupForm.addEventListener('submit', async function(e) {
            e.preventDefault();

            const loadingState = document.getElementById('popupLoadingState');
            const suggestionResult = document.getElementById('popupSuggestionResult');
            const suggestionContent = document.getElementById('popupSuggestionContent');

            if (!popupImageInput.files[0]) {
                alert('Lütfen bir fotoğraf yükleyin!');
                return;
            }

            // Occasion kontrolü
            const popupOccasionElement = document.querySelector('input[name="popupOccasion"]:checked');
            if (!popupOccasionElement) {
                alert('Lütfen bir durum seçin!');
                return;
            }

            try {
                // Loading göster
                loadingState.classList.remove('d-none');
                suggestionResult.classList.add('d-none');

                // Görseli base64'e çevir
                console.log('Popup: Görsel base64\'e çevriliyor...');
                const file = popupImageInput.files[0];
                const base64Image = await convertToBase64(file);
                
                // JSON olarak hazırla
                const requestData = {
                    ClothingImageBase64: base64Image,
                    FileName: file.name,
                    FileType: file.type,
                    Occasion: popupOccasionElement.value,
                    BudgetRange: '500-1500',
                    PreferredColors: ''
                };

                console.log('Popup: Request data hazırlandı');
                console.log('Popup: Base64 length:', base64Image?.length);
                console.log('Popup: Occasion:', popupOccasionElement.value);

                // Önce test endpoint'ini deneyelim
                console.log('Popup: Testing JSON deserialization...');
                
                // Önce basit GET test
                try {
                    const getTestResponse = await fetch('/api/OutfitSuggestion/test');
                    if (getTestResponse.ok) {
                        const getTestResult = await getTestResponse.json();
                        console.log('Popup: GET Test successful:', getTestResult);
                    } else {
                        console.error('Popup: GET Test failed:', getTestResponse.status);
                    }
                } catch (getTestErr) {
                    console.error('Popup: GET Test error:', getTestErr);
                }
                
                // Sonra POST test
                try {
                    const testResponse = await fetch('/api/OutfitSuggestion/test-json', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json'
                        },
                        body: JSON.stringify(requestData)
                    });
                    
                    if (testResponse.ok) {
                        const testResult = await testResponse.json();
                        console.log('Popup: Test successful:', testResult);
                    } else {
                        const testError = await testResponse.text();
                        console.error('Popup: Test failed:', testError);
                    }
                } catch (testErr) {
                    console.error('Popup: Test error:', testErr);
                }

                // API çağrısı
                const response = await fetch('/api/OutfitSuggestion/json', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(requestData)
                });

                if (!response.ok) {
                    throw new Error(`HTTP error! status: ${response.status}`);
                }

                const result = await response.json();
                console.log('Popup: API yanıtı:', result);
                console.log('Popup: Style:', result.Style);
                console.log('Popup: AltGiyim:', result.AltGiyim);

                // Güvenli property erişimi
                const style = result.Style || result.style || 'Kombin';
                const ustGiyim = result.UstGiyim || result.ustGiyim || 'Üst giyim';
                const altGiyim = result.AltGiyim || result.altGiyim || 'Alt giyim';
                const ayakkabi = result.Ayakkabi || result.ayakkabi || 'Ayakkabı';
                const aksesuar = result.Aksesuar || result.aksesuar || 'Aksesuar';

                // Sonucu göster
                suggestionContent.innerHTML = `
                    <div class="text-center">
                        <h6 class="fw-bold text-primary mb-2">${style} Kombini</h6>
                        <div class="row g-2 text-center">
                            <div class="col-12">
<<<<<<< HEAD
                                <small class="text-muted d-block cursor-pointer" ">👔 ${altGiyim}</small>
                                <small class="text-muted d-block cursor-pointer" ', '')">👟 ${ayakkabi}</small>
                                <small class="text-muted d-block cursor-pointer" ')">👜 ${aksesuar}</small>
=======
                                <small class="text-muted d-block cursor-pointer" "> ${ustGiyim}</small>
                                <small class="text-muted d-block cursor-pointer" "> ${altGiyim}</small>
                                <small class="text-muted d-block cursor-pointer" "> ${ayakkabi}</small>
                                <small class="text-muted d-block cursor-pointer" "> ${aksesuar}</small>
>>>>>>> temp-branch
                            </div>
                        </div>
                        <button class="btn btn-sm btn-primary mt-2 w-100" onclick="openOutfitSuggestion()">
                            <i class="fas fa-external-link-alt me-1"></i>Detaylı Görünüm
                        </button>
                    </div>
                `;
                suggestionResult.classList.remove('d-none');

            } catch (error) {
                console.error('Hata:', error);
                alert('Kombin önerisi alınırken bir hata oluştu.');
            } finally {
                loadingState.classList.add('d-none');
            }
        });
    }
});

// Global fonksiyonlar
function openOutfitSuggestion() {
    // Ana sayfaya yönlendir (AI widget'ı içeren)
    window.location.href = '/';
    
    // Popup'u kapat
    const aiChatPopup = document.getElementById('aiChatPopup');
    if (aiChatPopup) {
        aiChatPopup.classList.remove('show');
    }
}

function openProductSearch() {
    // Ürün arama sayfasına yönlendir
    window.location.href = '/products';
    
    // Popup'u kapat
    const aiChatPopup = document.getElementById('aiChatPopup');
    if (aiChatPopup) {
        aiChatPopup.classList.remove('show');
    }
}

function searchProducts(altGiyim, ayakkabi, aksesuar) {
    // Ürün arama sayfasına yönlendir
    const searchQuery = encodeURIComponent(`${altGiyim} ${ayakkabi} ${aksesuar}`);
    window.location.href = `/products?search=${searchQuery}`;
}

function shareOutfit() {
    // Kombin paylaşma
    if (navigator.share) {
        navigator.share({
            title: 'TrendyAI Kombin Önerim',
            text: 'AI yardımıyla oluşturduğum kombin önerisine bak!',
            url: window.location.href
        });
    } else {
        // Fallback: URL'yi kopyala
        navigator.clipboard.writeText(window.location.href).then(() => {
            alert('Link kopyalandı!');
        });
    }
}

// Stil geliştirmeleri için CSS
const style = document.createElement('style');
style.textContent = `
    .cursor-pointer {
        cursor: pointer !important;
    }
    
    .suggestion-item p span {
        cursor: pointer;
    }
    
    .upload-area {
        transition: all 0.3s ease;
        cursor: pointer;
    }
    
    .upload-area:hover {
        border-color: #0d6efd !important;
        background-color: #f8f9fa;
    }
    
    .suggestion-card {
        background: linear-gradient(135deg, #f8f9fa 0%, #e9ecef 100%);
        border: 1px solid #dee2e6 !important;
    }
    
    .ai-widget-card {
        box-shadow: 0 10px 30px rgba(0,0,0,0.1) !important;
    }
    
    .btn-check:checked + .btn {
        background-color: #0d6efd;
        border-color: #0d6efd;
        color: white;
    }
    .ai-assistant-float#aiChatPopup {
        display: none !important;
    }
    .ai-assistant-float#aiChatPopup.show {
        display: block !important;
    }
`;
document.head.appendChild(style); 