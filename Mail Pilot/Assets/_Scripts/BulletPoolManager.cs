using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Bonus - make this class a Singleton!

[System.Serializable]
public class BulletPoolManager
{
	public enum Bullet_Type
	{
		Type_1,
		Type_2,
		Type_3,
		Type_4
	}

	private static BulletPoolManager m_instance = null;

	private BulletPoolManager()
	{
		bullet_parent = GameObject.Find("bullet_pool");
		GameObject.DontDestroyOnLoad(bullet_parent);

		_BuildBulletPool();
	}

	public static BulletPoolManager Instance
	{
		get
		{
			if (m_instance == null)
			{
				m_instance = new BulletPoolManager();
				//GameObject.DontDestroyOnLoad(m_instance);
			}

			return m_instance;
		}
	}


	private int MaxBullets = 5;
	public GameObject bullet_template;

	private Bullet_Type active_type = Bullet_Type.Type_1;
	private GameObject bullet_parent;

	private Queue<GameObject> bullet_pool_1;
	private Queue<GameObject> bullet_pool_2;
	private Queue<GameObject> bullet_pool_3;
	private Queue<GameObject> bullet_pool_4;

	// Start is called before the first frame update
	void Start()
    {
		//_BuildBulletPool();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void _BuildBulletPool()
	{
		Transform parent = bullet_parent.transform;

		bullet_pool_1 = new Queue<GameObject>();

		for (int c = 0; c < MaxBullets; c++)
		{
			GameObject tmp_bullet = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Bullet1"));
			tmp_bullet.transform.parent = parent;
			tmp_bullet.GetComponent<BulletController>().type = Bullet_Type.Type_1;
			tmp_bullet.SetActive(false);
			bullet_pool_1.Enqueue(tmp_bullet);
		}

		bullet_pool_2 = new Queue<GameObject>();

		for (int c = 0; c < MaxBullets; c++)
		{
			GameObject tmp_bullet = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Bullet2"));
			tmp_bullet.transform.parent = parent;
			tmp_bullet.GetComponent<BulletController>().type = Bullet_Type.Type_2;
			tmp_bullet.SetActive(false);
			bullet_pool_2.Enqueue(tmp_bullet);
		}

		bullet_pool_3 = new Queue<GameObject>();

		for (int c = 0; c < MaxBullets; c++)
		{
			GameObject tmp_bullet = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Bullet3"));
			tmp_bullet.transform.parent = parent;
			tmp_bullet.GetComponent<BulletController>().type = Bullet_Type.Type_3;
			tmp_bullet.SetActive(false);
			bullet_pool_3.Enqueue(tmp_bullet);
		}

		bullet_pool_4 = new Queue<GameObject>();

		for (int c = 0; c < MaxBullets; c++)
		{
			GameObject tmp_bullet = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Bullet4"));
			tmp_bullet.transform.parent = parent;
			tmp_bullet.GetComponent<BulletController>().type = Bullet_Type.Type_4;
			tmp_bullet.SetActive(false);
			bullet_pool_4.Enqueue(tmp_bullet);
		}
	}

    //TODO: modify this function to return a bullet from the Pool
    public GameObject GetBullet()
    {	
		GameObject bullet;

		Queue<GameObject> bullet_pool = GetPool(active_type);

		if (bullet_pool.Count > 0)
			bullet = bullet_pool.Dequeue();
		else
		{
			bullet = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Bullet" + ((int)active_type + 1).ToString()));
			bullet.GetComponent<BulletController>().type = active_type;
			bullet.transform.parent = bullet_parent.transform;
		}

		bullet.SetActive(true);

		return bullet;
    }

    //TODO: modify this function to reset/return a bullet back to the Pool 
    public void ResetBullet(GameObject bullet)
    {
		Queue<GameObject> bullet_pool = GetPool(bullet.GetComponent<BulletController>().type);

		bullet.SetActive(false);
		bullet_pool.Enqueue(bullet);
    }

	public int PoolSize(Bullet_Type t)
	{
		Queue<GameObject> bullet_pool = GetPool(t);
		return bullet_pool.Count;
	}

	public bool isEmpty(Bullet_Type t)
	{
		Queue<GameObject> bullet_pool = GetPool(t);

		return bullet_pool.Count == 0;
	}

	private Queue<GameObject> GetPool(Bullet_Type t)
	{
		Queue<GameObject> bullet_pool;

		switch (t)
		{
			case Bullet_Type.Type_1:
			default:
				bullet_pool = bullet_pool_1;
				break;
			case Bullet_Type.Type_2:
				bullet_pool = bullet_pool_2;
				break;
			case Bullet_Type.Type_3:
				bullet_pool = bullet_pool_3;
				break;
			case Bullet_Type.Type_4:
				bullet_pool = bullet_pool_4;
				break;
		}

		return bullet_pool;
	}

	public void ToggleBulletType()
	{
		int cur = (int)active_type;

		cur++;

		if (cur >= 4)
		{
			cur = 0;
		}

		active_type = (Bullet_Type)cur;
	}

	public void SetMaxBullets(int new_max)
	{
		MaxBullets = new_max;

		for(int c = 0; c < 4; c++)
		{
			Queue<GameObject> pool = GetPool((Bullet_Type)c);

			if (pool.Count > MaxBullets)
			{
				while (pool.Count > MaxBullets)
				{
					GameObject.Destroy(pool.Dequeue());
				}
}
			else
			{
				Transform parent = bullet_parent.transform;
				while (pool.Count < MaxBullets)
				{
					GameObject tmp = GameObject.Instantiate((GameObject)Resources.Load("Prefabs/Bullet" + (c+1).ToString()));
					tmp.transform.parent = parent;
					tmp.GetComponent<BulletController>().type = (Bullet_Type)c;
					tmp.SetActive(false);
					pool.Enqueue(tmp);
				}
			}
		}
	}
}
