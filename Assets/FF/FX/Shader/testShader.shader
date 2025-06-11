Shader "Custom/testShader" // Назва шейдера, як він буде відображатися в Unity
{
    Properties // Блок властивостей, які можна змінювати в інспекторі матеріалу
    {
        _Color ("Color", Color) = (1,1,1,1) // Колір, який буде використовуватися для заливки
        _Color2 ("Color2", Color) = (1,1,1,1)
        _Alpha ("Alpha", Range(0, 1)) = 1 // Альфа-канал для прозорості (не використовується в цьому шейдері) 
        _MainTex ("Texture", 2D) = "white" {} // Основна текстура (наприклад, спрайт)
        _float1 ("float1", Range(0, 100)) = 10
        _float2 ("float2", Range(0, 100)) = 10 
        _lol ("lol", Range(0, 1)) = 0.05
    }
    SubShader // Основний блок, який містить інструкції для рендеру
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" } // Вказує, що шейдер прозорий (малюється після непрозорих)
        Pass // Один прохід рендеру (може бути кілька Pass)
        {
            Blend SrcAlpha OneMinusSrcAlpha // Налаштування прозорості: альфа-композиція
            ZWrite Off // Додаємо це, щоб прозорі ділянки не писали в depth buffer

            CGPROGRAM // Початок блоку коду на HLSL/CG

            #pragma vertex vert   // Вказує, що функція vert — вершинний шейдер
            #pragma fragment frag // Вказує, що функція frag — фрагментний (піксельний) шейдер
            // Структура для вхідних даних у вершинний шейдер
            struct appdata
            {
                float4 vertex : POSITION; // Позиція вершини у просторі моделі
                float2 uv : TEXCOORD0;    // Текстурні координати (UV)
            };

            // Структура для передачі даних з вершинного у фрагментний шейдер
            struct v2f
            {
                float4 pos : SV_POSITION; // Позиція вершини у просторі кліпу (екрану)
                float2 uv : TEXCOORD0;    // Текстурні координати (UV)
            };

            sampler2D _MainTex; // Текстура, яку будемо використовувати (оголошена у Properties)
            fixed4 _Color;      // Колір, який будемо використовувати (оголошений у Properties)
            fixed4 _Color2;     // Другий колір, який будемо використовувати (оголошений у Properties)
            float _Alpha;
            float _float1;
            float _float2;
            float _lol;

            // Вершинний шейдер: трансформує вершини у простір екрану та передає UV
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); // Перетворення позиції у простір кліпу (екрану)
                o.uv = v.uv;                            // Передаємо UV далі
                return o;
            }

            // Фрагментний (піксельний) шейдер: визначає колір кожного пікселя
            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y += cos(_Time.y + uv.x * _float1) * _lol;
                uv.x += sin(_Time.y + uv.y * _float2) * _lol; 

                /*float rowIndex = floor(uv.y * _float1*10); // Кількість рядків (20 можна змінити)
                float offset = _lol; // Величина зміщення (можна змінити)
                
                // Зміщуємо парні рядки вліво, непарні вправо
                if (fmod(rowIndex, 2.0) == 0)
                    uv.x -= offset;
                else
                    uv.x += offset;
                    */
                fixed4 texCol = tex2D(_MainTex, uv); // Зчитуємо колір з текстури за координатами UV
                if (texCol.a == 0) discard;             // Якщо альфа (прозорість) == 0, не малюємо цей піксель
                if (texCol.a == 1) return texCol;
                if (texCol.a >= _Alpha) return _Color2 * texCol.a;  
                return _Color * texCol.a;              // Повертаємо колір заливки з урахуванням прозорості текстури

            }
            ENDCG // Кінець блоку коду на HLSL/CG
        }
    }
    FallBack "Diffuse" // Якщо шейдер не підтримується, використовувати стандартний Diffuse
}
